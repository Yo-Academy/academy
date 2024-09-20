using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class remove_multitenent_academy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Academies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Academies",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
