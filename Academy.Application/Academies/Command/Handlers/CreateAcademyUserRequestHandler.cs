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
    public class CreateAcademyUserRequestHandler : IRequestHandler<CreateAcademyUserRequest, Result>
    {
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;

        public CreateAcademyUserRequestHandler(IUserService userService, ITenantService tenantService)
        {
            _userService = userService;
            _tenantService = tenantService;
        }
        public async Task<Result> Handle(CreateAcademyUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _tenantService.CreateWithUsersAsync(request, cancellationToken);
                return Result.Succeed(String.Format(DbRes.T("UserInsertedSuccessFully"), request.Role));
            }
            catch (Exception ex)
            {

            }
            return Result.Fail();
        }
    }

}
