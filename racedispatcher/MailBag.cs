using Microsoft.AspNetCore.WebUtilities;
using racedispatcher.Protos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace racedispatcher
{
    public class MailBag
    {
        static private MailBag singletonMailbag;
        private BufferBlock<NewImageRequest> analyzerMailbox = new BufferBlock<NewImageRequest>();
        private BufferBlock<NewLineCrossingEvent> directorMailbox = new BufferBlock<NewLineCrossingEvent>();
        public bool TerminateFlagged = false;
        private MailBag() { }

        static public MailBag Get()
        {
            if(singletonMailbag == null)
            {
                singletonMailbag = new MailBag();
            }

            return singletonMailbag;
        }

        internal async Task AddNewImage(NewImageRequest nir)
        {
            await analyzerMailbox.SendAsync(nir);
        }

        internal async Task AddLineCrossing(NewLineCrossingEvent nlce)
        {
            await directorMailbox.SendAsync(nlce);
        }

        public async Task AnalyzerMessageLoop(Grpc.Core.IServerStreamWriter<NewImageRequest> responseStream)
        {
            while(!TerminateFlagged)
            {
                await responseStream.WriteAsync(await analyzerMailbox.ReceiveAsync());
            }
        }

        public async Task DirectorMessageLoop(Grpc.Core.IServerStreamWriter<NewLineCrossingEvent> responseStream)
        {
            while (!TerminateFlagged)
            {
                await responseStream.WriteAsync(await directorMailbox.ReceiveAsync());
            }
        }
    }
}
