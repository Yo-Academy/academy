using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Common.Helpers;
using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Identity.Users;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Shared;
using Mapster;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.IO;
using System.Numerics;
using static Academy.Shared.Constants;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class CreateAcademyUserRequestHandler : IRequestHandler<CreateAcademyUserRequest, Result<UserDetailsDto>>
    {
        private readonly IUserService _userService;

        public CreateAcademyUserRequestHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<UserDetailsDto>> Handle(CreateAcademyUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Result<UserDetailsDto> user = await _userService.CreateAsyncWithTenantId(request, request.Origin);
                return user;
            }
            catch(Exception ex)
            {

            }
            return Result.Fail();
        }
    }

}
