using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class add_foreign_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_AcademyId",
                table: "AcademySportsMapping",
                column: "AcademyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMapping_Academies_AcademyId",
                table: "AcademySportsMapping",
                column: "AcademyId",
                principalTable: "Academies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Academies_AcademyId",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_AcademyId",
                table: "AcademySportsMapping");
        }
    }
}
