using Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec;

namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands
{
    public class CreateHeadQuarterRequestValidator : CustomValidator<CreateHeadQuarterRequest>
    {
        public CreateHeadQuarterRequestValidator(ICurrentUser currentUser, IRepository<HeadQuarter> headRepo, IReadRepository<HeadQuarterPhone> headPhoneRepo,
             IStringLocalizer<CreateHeadQuarterRequestValidator> T)
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (name, ct) => await headRepo.GetBySpecAsync(new HeadQuarterByNameSpec(name, currentUser.GetBusinessId()), ct) is null)
                .WithMessage((_, name) => T["HeadQuarter {0} already exist", name]);

            RuleFor(e => e.City)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .WithMessage(T["City can't be null"]);

            RuleFor(e => e.Region)
               .NotEmpty()
               .NotNull()
               .MaximumLength(100)
               .WithMessage(T["Region can't be null"]);

            RuleFor(e => e.Street)
               .NotEmpty()
               .NotNull()
               .MaximumLength(100)
               .WithMessage(T["Street can't be null"]);

            RuleFor(e => e.Building)
               .NotEmpty()
               .NotNull()
               .MaximumLength(100)
               .WithMessage(T["Building can't be null"]);

            RuleFor(e => e.Phones)
               .MustAsync(async (_, phones, ct) => await CheckPhones(phones, headPhoneRepo, T, currentUser.GetBusinessId(), ct));
        }

        private static async Task<bool> CheckPhones(List<string> phones, IReadRepository<HeadQuarterPhone> headPhoneRepo,
            IStringLocalizer<CreateHeadQuarterRequestValidator> T, Guid businessId, CancellationToken ct)
        {
            if (phones.Count < 2)
                throw new ConflictException(T["You need to add two phone numbers at least"]);
            else
            {
                // Check If The List Is Unique:
                for (int i = 0; i < phones.Count; i++)
                {
                    for (int j = i + 1; j < phones.Count; j++)
                    {
                        if (phones[i].Equals(phones[j]))
                            throw new ConflictException(T["Phones must be unique"]);
                    }
                }

                foreach (string phone in phones)
                {

                    if (phone.Length != 11)
                        if (phone.Length != 7)
                            throw new ConflictException(T["Phone {0} Not Valid!", phone]);

                    var checkPhone = await headPhoneRepo.GetBySpecAsync(new HeadQuarterPhoneByPhoneAndBusinessIdSepc(phone, businessId), ct);
                    if (checkPhone != null) throw new ConflictException(T["Phone {0} already exists.", phone]);
                }
            }

            return true;
        }
    }
}
