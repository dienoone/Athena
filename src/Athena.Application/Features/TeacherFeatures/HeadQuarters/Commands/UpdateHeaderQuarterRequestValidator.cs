using Athena.Application.Features.TeacherFeatures.HeadQuarters.Spec;

namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands
{
    public class UpdateHeaderQuarterRequestValidator : CustomValidator<UpdateHeaderQuarterRequest>
    {
        public UpdateHeaderQuarterRequestValidator(IReadRepository<HeadQuarter> repository, IReadRepository<HeadQuarterPhone> headPhonesRepo,
            IStringLocalizer<CreateHeadQuarterRequestValidator> T, ICurrentUser currentUser)
        {

            RuleFor(e => e.Name)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (head, name, ct) =>
                    await repository.GetBySpecAsync(new HeadQuarterByNameSpec(name, currentUser.GetBusinessId()), ct)
                        is not HeadQuarter existingHeadQuarter || existingHeadQuarter.Id == head.Id)
                .WithMessage((_, name) => T["HeadQuarter {0} already exist.", name]);

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
                .MustAsync(async (request, phones, ct) => await CheckUpdatePhones(phones, headPhonesRepo, T, request.Id, ct));

            RuleFor(e => e.NewPhone)
               .MustAsync(async (request, newPhone, ct) => await CheckNewPhones(newPhone, headPhonesRepo, T, request.Id, ct));

        }

        private static async Task<bool> CheckUpdatePhones(List<UpdateHeadQuarterPhones> phones, IReadRepository<HeadQuarterPhone> headPhonesRepo,
            IStringLocalizer<CreateHeadQuarterRequestValidator> T, Guid headId, CancellationToken ct)
        {
            if (phones.Count < 2)
                throw new InternalServerException(T["You need to add two phone numbers at least"]);
            else
            {
                foreach (var phone in phones)
                {
                    if (!phone.IsDeleted)
                    {
                        var checkPhone = await headPhonesRepo.GetBySpecAsync(new HeadQuarterPhoneByPhoneAndHeadIdSepc(phone.Phone, headId), ct);
                        if (checkPhone != null && checkPhone!.Id != phone.Id)
                        {
                            throw new ConflictException(T["Phone {0} already exists", checkPhone.Phone]);
                        }
                    }
                }
            }
            return true;
        }

        private static async Task<bool> CheckNewPhones(string? newPhone, IReadRepository<HeadQuarterPhone> headRepo,
            IStringLocalizer<CreateHeadQuarterRequestValidator> T, Guid headId, CancellationToken ct)
        {
            if (newPhone != null)
            {
                if (newPhone.Length != 11)
                    if (newPhone.Length != 7)
                        throw new InternalServerException(T["Phone {0} Not Valid!", newPhone.Length]);

                var isExist = await headRepo.GetBySpecAsync(new HeadQuarterPhoneByPhoneAndHeadIdSepc(newPhone, headId), ct);
                if (isExist != null) throw new InternalServerException(T["Phone {0} already exists.", newPhone]);
            }
            return true;
        }
    }
}
