namespace Academy.Application.Identity.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateRoleRequestValidator : CustomValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator(IRoleService roleService) =>
            RuleFor(r => r.Name)
                .NotEmpty()
                .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name))
                    .WithMessage(DbRes.T("RoleAlreadyExistMsg"));
    }
}
