using Academy.Domain.Mailing;
using ManagedCode.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Common.Mailing
{
    public interface IEmailHelper
    {
        string GenerateEmailTemplate<T>(string template, T mailTemplateModel);

        Task<Result<bool>> SendAsync(MailRequest request);
    }
}