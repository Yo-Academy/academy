using Academy.Application.Common.Mailing;
using Academy.Domain.Mailing;
using ManagedCode.Communication;
using Microsoft.AspNetCore.Hosting;
using RazorEngineCore;
using System.Text;


namespace Academy.Infrastructure.Mailing
{
    public abstract class BaseEmailHelper : IEmailHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BaseEmailHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string GenerateEmailTemplate<T>(string template, T mailTemplateModel)
        {
            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

            return modifiedTemplate.Run(mailTemplateModel);
        }

        public async Task<Result<bool>> SendAsync(MailRequest request)
        {
            return await SendMailAsync(request);
        }

        protected abstract Task<Result<bool>> SendMailAsync(MailRequest request);
    }


}
