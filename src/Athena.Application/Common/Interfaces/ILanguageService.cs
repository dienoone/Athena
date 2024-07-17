namespace Athena.Application.Common.Interfaces
{
    public interface ILanguageService
    {
        string GetCurrentLanguage();
        void SetCurrentLanguage(string language);
    }
}
