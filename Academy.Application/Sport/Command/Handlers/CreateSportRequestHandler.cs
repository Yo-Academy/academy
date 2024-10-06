using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Persistence.Repository;
using Academy.Application.Sport.Command.Models;
using Academy.Application.Sport.Dto;
using Academy.Domain.Entities;
using Mapster;
using static Academy.Shared.Constants;

namespace Academy.Application.Sport.Command.Handlers
{
    public class CreateSportRequestHandler : IRequestHandler<CreateSportRequest, Result<SportsDto>>
    {
        private readonly IRepository<Sports> _repository;
        private readonly IStorage _storage;

        public CreateSportRequestHandler(IRepository<Sports> repository, IStorage storage)
        {
            _repository = repository;
            _storage = storage;
        }
        public async Task<Result<SportsDto>> Handle(CreateSportRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            string imageName = string.Empty;

            if (request.Image != null)
            {
                imageName = string.Format("{0}_{1}_sport{2}", id, request.Name, request.Image.GetFileExtension());
                using (var stream = request.Image.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = imageName, Directory = S3Directory.Sports }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Sports Image file uploaded issue.");
                    }
                }
            }

            Sports Sport = new Sports(id, request.Name, imageName, request.IsActive);

            //Inserts RequirementSet Record
            var responseSport = await _repository.AddAsync(Sport);

            return Result.Succeed(responseSport.Adapt<SportsDto>());
        }
    }

}
