namespace Athena.Application.Features.StudentFeatures.Base.Spec
{
    public class ResetPasswordTokenByCodeAndEmailSpec : Specification<ResetPasswordToken>, ISingleResultSpecification
    {
        public ResetPasswordTokenByCodeAndEmailSpec(string email, string code, DateTime date) =>
            Query.Where(e => e.Email == email && e.ExpirationTime > date && e.Code == code && !e.Used);
    }
}
