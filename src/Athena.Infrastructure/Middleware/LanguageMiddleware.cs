using Microsoft.AspNetCore.Http;

namespace Athena.Infrastructure.Middleware
{
    public class LanguageMiddleware : IMiddleware
    {
        private readonly ILanguageService _languageService;
        private readonly string defaultLanguage = "ar"; // Set your default language here

        public LanguageMiddleware(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Access the language from the header or any other source
            var language = context.Request.Headers["Accept-Language"].ToString();
            Console.WriteLine(language);
            // Set the language using the language service
            _languageService.SetCurrentLanguage(string.IsNullOrEmpty(language) ? defaultLanguage : language);

            // Call the next middleware in the pipeline
            await next(context);
        }
    }
}
