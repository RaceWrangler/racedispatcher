using racedispatcher.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace racedispatcher.Services
{
    public class DirectorRegistrationService : DirectorRegistration.DirectorRegistrationBase
    {
        public override async Task DirectorRegistration(RegisterToRecieveAnalyzerMessages msg,
            Grpc.Core.IServerStreamWriter<NewLineCrossingEvent> responseStream,
            Grpc.Core.ServerCallContext context)
        {
            await MailBag.Get().DirectorMessageLoop(responseStream);
            await Task.Delay(-1);
        }
    }
}
