using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Microsoft.AspNetCore.Http;

namespace Academy.Application.Features.Storage
{
    public class UploadFileRequest : IRequest<bool>
    {
        public IFormFile FormFile { get; set; }

        public string FullPath { get; set; }
    }

    public class UploadFileRequestValidator : CustomValidator<UploadFileRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(p => p.FullPath)
           .NotEmpty()
               .WithMessage(DbRes.T("FileFullpathEmptyMsg"));

            RuleFor(p => p.FormFile)
            .NotEmpty()
                .WithMessage(DbRes.T("FileEmptyMsg"));
        }
    }

    public class UploadFileRequestHandler : IRequestHandler<UploadFileRequest, bool>
    {
        private readonly IStorage _storage;

        public UploadFileRequestHandler(IStorage storage) =>
            _storage = storage;

        public async Task<bool> Handle(UploadFileRequest request, CancellationToken cancellationToken)
        {
            var res = await _storage.UploadAsync(request.FormFile.Resize(400, 300), new UploadOptions() { FileName = request.FullPath }, cancellationToken);
            return res.IsSuccess;
        }
    }
}
