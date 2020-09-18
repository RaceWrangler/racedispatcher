using racedispatcher.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace racedispatcher.Services
{
    public class AnalyzerRegistrationService : AnalyzerRegistration.AnalyzerRegistrationBase
    {
        public override async Task AnalyzerRegistration(RegisterToRecieveHandlerMessages msg,
            Grpc.Core.IServerStreamWriter<NewImageRequest> responseStream,
            Grpc.Core.ServerCallContext context)
        {
            await MailBag.Get().AnalyzerMessageLoop(responseStream);
        }
    }
}
