using Grpc.Core;
using racedispatcher.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace racedispatcher.Services
{
    public class HandlerMessageService : HandlerMessage.HandlerMessageBase
    {
        public HandlerMessageService() { }

        public override async Task<NewImageResponse> AnalyzeNewImage(NewImageRequest nir, ServerCallContext context)
        {
            await MailBag.Get().AddNewImage(nir);
            return new NewImageResponse();
        }
    }
}
