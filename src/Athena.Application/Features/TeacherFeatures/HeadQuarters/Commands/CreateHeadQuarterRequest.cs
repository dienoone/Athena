namespace Athena.Application.Features.TeacherFeatures.HeadQuarters.Commands
{
    public class CreateHeadQuarterRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Region { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Building { get; set; } = default!;
        public List<string> Phones { get; set; } = default!;
    }

    public class CreateHeadQuarterRequestHandler : IRequestHandler<CreateHeadQuarterRequest, Guid>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<HeadQuarter> _repository;
        private readonly IRepository<HeadQuarterPhone> _headPhoneRepo;
        private readonly IStringLocalizer<CreateHeadQuarterRequestHandler> _t;

        public CreateHeadQuarterRequestHandler(ICurrentUser currentUser, IRepository<HeadQuarter> repository, IRepository<HeadQuarterPhone> headPhoneRepo, 
            IStringLocalizer<CreateHeadQuarterRequestHandler> t)
        {
            _currentUser = currentUser;
            _repository = repository;
            _headPhoneRepo = headPhoneRepo;
            _t = t;
        }

        public async Task<Guid> Handle(CreateHeadQuarterRequest request, CancellationToken cancellationToken)
        {
            HeadQuarter headQuarter = new(request.Name, request.City, request.Region, request.Street, request.Building, _currentUser.GetUserId(), _currentUser.GetBusinessId());
            await _repository.AddAsync(headQuarter, cancellationToken);

            if (request.Phones != null)
            {
                if (request.Phones.Count > 0)
                {
                    foreach (string phone in request.Phones)
                    {
                        HeadQuarterPhone headQuarterPhone = new(phone, headQuarter.Id, _currentUser.GetBusinessId());
                        await _headPhoneRepo.AddAsync(headQuarterPhone, cancellationToken);
                    }
                }
            }

            return headQuarter.Id;
        }
    }
}
