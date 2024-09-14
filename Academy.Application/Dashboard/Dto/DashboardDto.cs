namespace Academy.Application.Dashboard.Dto
{
    public class DashboardDto
    {
        public DefaultIdType Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int ActiveAcademyPlayers { get; set; }

        public int ActiveMembers { get; set; }

        public int ActiveUsers { get; set; }

        public int Users { get; set;  }
    }
}
