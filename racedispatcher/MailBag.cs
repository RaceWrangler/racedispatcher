using Microsoft.AspNetCore.WebUtilities;
using racedispatcher.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace racedispatcher
{
    public class MailBag
    {
        static private MailBag singletonMailbag;
        private Queue<NewImageRequest> analyzerMailbox = new Queue<NewImageRequest>();
        private Queue<NewLineCrossingEvent> directorMailbox = new Queue<NewLineCrossingEvent>();
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

        public async Task AnalyzerMessageLoop(Grpc.Core.IServerStreamWriter<NewImageRequest> responseStream)
        {
            while(!TerminateFlagged)
            {
                if (analyzerMailbox.Count > 0)
                {
                    await responseStream.WriteAsync(analyzerMailbox.Dequeue());
                }
                await Task.Delay(10);
            }
        }

        public async Task DirectorMessageLoop(Grpc.Core.IServerStreamWriter<NewLineCrossingEvent> responseStream)
        {
            while (!TerminateFlagged)
            {
                if (directorMailbox.Count > 0)
                {
                    await responseStream.WriteAsync(directorMailbox.Dequeue());
                }
                await Task.Delay(10);
            }
        }
    }
}
