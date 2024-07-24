namespace Academy.Application.Identity.Roles
{
    public class UpdateRoleRequest
    {
        public DefaultIdType Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class UpdateRoleRequestValidator : CustomValidator<UpdateRoleRequest>
    {
        public UpdateRoleRequestValidator(IRoleService roleService) =>
            RuleFor(r => r.Name)
                .NotEmpty()
                .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
                    .WithMessage(DbRes.T("RoleAlreadyExistMsg"));
    }
}
