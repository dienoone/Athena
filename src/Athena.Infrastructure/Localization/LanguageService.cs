namespace Athena.Infrastructure.Localization
{
    public class LanguageService : ILanguageService
    {
        private string currentLanguage = default!;

        public string GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public void SetCurrentLanguage(string language)
        {
            currentLanguage = language;
        }
    }
}
