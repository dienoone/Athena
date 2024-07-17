namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands
{
    public record UpdateHeadQuarterPhones(Guid Id, string Phone, bool IsDeleted);
    public class UpdateHeaderQuarterRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Region { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Building { get; set; } = default!;

        public List<UpdateHeadQuarterPhones> Phones { get; set; } = null!;
        public string? NewPhone { get; set; }
    }

    public class UpdateHeaderQuarterRequestHandler : IRequestHandler<UpdateHeaderQuarterRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<HeadQuarter> _headRepo;
        private readonly IRepository<HeadQuarterPhone> _headPhoneRepo;
        private readonly IStringLocalizer<UpdateHeaderQuarterRequestHandler> _t;

        public UpdateHeaderQuarterRequestHandler(ICurrentUser currentUser, IRepository<HeadQuarter> headRepo, IRepository<HeadQuarterPhone> headPhoneRepo,
            IStringLocalizer<UpdateHeaderQuarterRequestHandler> t) =>
            (_headRepo, _headPhoneRepo, _t, _currentUser) = (headRepo, headPhoneRepo, t, currentUser);
        public async Task<Guid> Handle(UpdateHeaderQuarterRequest request, CancellationToken cancellationToken)
        {
            var head = await _headRepo.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(_t["HeadQuarter {0} dosen't exist.", request.Id]);

            foreach (var phone in request.Phones)
            {
                var existedPhone = await _headPhoneRepo.GetByIdAsync(phone.Id, cancellationToken);
                _ = existedPhone ?? throw new NotFoundException(_t["Phone {0} Not Found!", phone.Id]);

                if (phone.IsDeleted)
                {
                    await _headPhoneRepo.DeleteAsync(existedPhone, cancellationToken);
                }
                else
                {
                    existedPhone.Update(phone.Phone);
                    await _headPhoneRepo.UpdateAsync(existedPhone, cancellationToken);
                }
            }

            if (request.NewPhone != null)
            {
                HeadQuarterPhone headQuarterPhone = new(request.NewPhone, request.Id, _currentUser.GetBusinessId());
                await _headPhoneRepo.AddAsync(headQuarterPhone, cancellationToken);
            }

            head.Update(request.Name, request.City, request.Region, request.Street, request.Building);
            await _headRepo.UpdateAsync(head, cancellationToken);

            return head.Id;
        }

    }
}
