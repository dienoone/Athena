using Athena.Application.Common.Mailing;
using Microsoft.AspNetCore.Hosting;
using RazorEngineCore;
using System.Text;

namespace Athena.Infrastructure.Mailing
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IWebHostEnvironment _env;

        public EmailTemplateService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            string template = GetTemplate(templateName);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

            return modifiedTemplate.Run(mailTemplateModel);
        }

        public string GetTemplate(string templateName)
        {
            string tmplFolder = Path.Combine(_env.ContentRootPath, "Email Templates");
            string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            string mailText = sr.ReadToEnd();
            sr.Close();

            return mailText;
        }
    }
}
