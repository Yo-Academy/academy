using Academy.Application.EmailTemplates;
using Academy.Infrastructure.Persistence.Context;
using Academy.Infrastructure.Persistence.Initialization;

namespace Academy.Infrastructure.EmailTemplates
{
    public class EmailTemplateSeeder : ICustomSeeder
    {
        private readonly ApplicationDbContext _db;

        public EmailTemplateSeeder(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (!await _db.EmailTemplates.Where(x => x.TemplateCode.Trim() == "CNFEM").AnyAsync())
            {
                var confirmEmailTemplate = new EmailTemplateDto
                {
                    Id = Guid.NewGuid(),
                    TemplateCode = "CNFEM",
                    Name = "Confirm Email",
                    Subject = "Confirm Email",
                    Body = "<table style=\"width: 100%;\"><tbody><tr><td align=\"center\"><table style=\"max-width: 600px; width:600px; margin: auto; color: #233443; font-family: Helvetica, Arial, sans-serif; font-size: 14px;  border: 1px solid #e7e7e7;\" cellspacing=\"0\" cellpadding=\"0\"> <tbody><!-- || Logo || --><tr><td style='padding: 20px; background-color: #fff; border-bottom: 3px solid #000000'><table style='width: 100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='vertical-align: middle;'>#lOGO</td></tr></tbody></table></td></tr><!-- || Body Part || --><tr><td style='padding: 50px 20px 20px 20px; background-color: #ffffff; border: 0px;'><table style='width: 100%;'><tbody><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 16px; color: #233443; font-weight: bold; line-height: 22px; font-style: normal; padding-bottom: 10px;'><p style='margin: 0px;'>Dear @Model?.UserName,</p></td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px; padding-bottom: 5px;'><p style='margin: 0px;'>We're excited to have you get started. First, you need to confirm your email - @Model?.Email for this account. Just press the button below.</p></td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px; padding-bottom: 5px;'><table><tr><td bgcolor=\"#000000\" style=\"padding-left: 30px; padding-right: 30px; padding-top: 15px; padding-bottom: 15px;  text-align:center; border-radius:4px;\"><a style=\"font-size: 14px; text-decoration: none; background: #000000; color: #ffffff; font-family: Calibri, -apple-system, sans-serif;\" href=\"@Model?.Url\">Confirm Email</a></td></tr></table></td></tr><!-- || Best regards || --><tr><td style=\"border:0;padding-top:0;\"><table style=\"width: 100%;border:0;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; padding-bottom: 5px;'><br />Best regards,</td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px;padding:0;'><strong>#Company</strong></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>"
                };
                await _db.EmailTemplates.AddAsync(confirmEmailTemplate.Adapt<EmailTemplate>());
            }

            if (!await _db.EmailTemplates.Where(x => x.TemplateCode == "RSPW").AnyAsync())
            {
                var resetPasswordTemplate = new EmailTemplateDto
                {
                    Id = Guid.NewGuid(),
                    TemplateCode = "RSPW",
                    Name = "Reset Password",
                    Subject = "Reset Password",
                    Body = "<table style=\"width: 100%;\"><tbody><tr><td align=\"center\"><table style=\"max-width: 600px; width:600px; margin: auto; color: #233443; font-family: Helvetica, Arial, sans-serif; font-size: 14px;  border: 1px solid #e7e7e7;\" cellspacing=\"0\" cellpadding=\"0\"><tbody><!-- || Logo || --><tr><td style='padding: 20px; background-color: #fff; border-bottom: 3px solid #000000'><table style='width: 100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='vertical-align: middle;'>#lOGO</td></tr></tbody></table></td></tr><!-- || Body Part || --><tr><td style='padding: 50px 20px 20px 20px; background-color: #ffffff; border: 0px;'><table style='width: 100%;'><tbody><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 16px; color: #233443; font-weight: bold; line-height: 22px; font-style: normal; padding-bottom: 10px;'><p style='margin: 0px;'>Dear @Model?.UserName,</p></td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px; padding-bottom: 5px;'><p style='margin: 0px;'>To reset your password, please click on the button below:</p></td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px; padding-bottom: 5px;'><table><tr><td bgcolor=\"#000000\" style=\"padding-left: 30px; padding-right: 30px; padding-top: 15px; padding-bottom: 15px;  text-align:center; border-radius:4px;\"><a style=\"font-size: 14px; text-decoration: none; background: #000000; color: #ffffff; font-family: Calibri, -apple-system, sans-serif;\" href=\"@Model?.Url\">Reset Password</a></td></tr></table></td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px; padding-bottom: 5px;'><p style='margin: 0px;'>If you did not request a password reset, you can safely ignore this email.</p></td></tr><!-- || Best regards || --><tr><td style=\"border:0;padding-top:0;\"><table style=\"width: 100%;border:0;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; padding-bottom: 5px;'><br />Best regards,</td></tr><tr><td style='font-family: Calibri, -apple-system, sans-serif; font-size: 14px; color: #233443; font-weight: normal; line-height: 20px; font-style: normal; margin: 0px;padding:0;'><strong>#Company</strong></td></tr></table></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>"
                };
                await _db.EmailTemplates.AddAsync(resetPasswordTemplate.Adapt<EmailTemplate>());
            }
            await _db.SaveChangesAsync(cancellationToken);

        }
    }
}
