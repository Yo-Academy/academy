using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class add_UserInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    FatherName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DateOfJoin = table.Column<DateOnly>(type: "date", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    PinCode = table.Column<int>(type: "integer", nullable: false),
                    ContactNo = table.Column<int>(type: "integer", nullable: false),
                    ProfilePic = table.Column<string>(type: "text", nullable: true),
                    IsACtive = table.Column<bool>(type: "boolean", nullable: false),
                    SportsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoachingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentFee = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    last_modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInfo_Coaching_CoachingId",
                        column: x => x.CoachingId,
                        principalTable: "Coaching",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInfo_PlanType_PlanTypeId",
                        column: x => x.PlanTypeId,
                        principalTable: "PlanType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInfo_Sports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
