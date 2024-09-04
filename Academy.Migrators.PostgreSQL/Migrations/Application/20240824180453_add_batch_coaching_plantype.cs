using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class add_batch_coaching_plantype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coaching",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Coaching", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_PlanType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    SportsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchName = table.Column<string>(type: "text", nullable: false),
                    CoachingId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Days = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Batch", x => x.id);
                    table.ForeignKey(
                        name: "FK_Batch_Coaching_CoachingId",
                        column: x => x.CoachingId,
                        principalTable: "Coaching",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Batch_Sports_SportsId",
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
                name: "Batch");

            migrationBuilder.DropTable(
                name: "PlanType");

            migrationBuilder.DropTable(
                name: "Coaching");
        }
    }
}
