namespace Academy.Application.Permission.Dto
{
    public class PermissionListDto
    {
        public DefaultIdType Id { get; set; }

        public string Action { get; set; }

        public string Resource { get; set; }

        public bool IsGranted { get; set; }
    }
}
