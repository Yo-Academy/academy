using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class dropAcademyIdFromAcademies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Academies_AcademyId",
                table: "AcademySportsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportsId",
                table: "AcademySportsMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademySportsMapping",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_AcademyId",
                table: "AcademySportsMapping");

            migrationBuilder.RenameTable(
                name: "AcademySportsMapping",
                newName: "AcademySportsMappings");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMapping_SportsId",
                table: "AcademySportsMappings",
                newName: "IX_AcademySportsMappings_SportsId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMapping_last_modified_by",
                table: "AcademySportsMappings",
                newName: "IX_AcademySportsMappings_last_modified_by");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMapping_created_by",
                table: "AcademySportsMappings",
                newName: "IX_AcademySportsMappings_created_by");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademiesId",
                table: "AcademySportsMappings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademySportsMappings",
                table: "AcademySportsMappings",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMappings_AcademiesId",
                table: "AcademySportsMappings",
                column: "AcademiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMappings_Academies_AcademiesId",
                table: "AcademySportsMappings",
                column: "AcademiesId",
                principalTable: "Academies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMappings_Sports_SportsId",
                table: "AcademySportsMappings",
                column: "SportsId",
                principalTable: "Sports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMappings_Academies_AcademiesId",
                table: "AcademySportsMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMappings_Sports_SportsId",
                table: "AcademySportsMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademySportsMappings",
                table: "AcademySportsMappings");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMappings_AcademiesId",
                table: "AcademySportsMappings");

            migrationBuilder.DropColumn(
                name: "AcademiesId",
                table: "AcademySportsMappings");

            migrationBuilder.RenameTable(
                name: "AcademySportsMappings",
                newName: "AcademySportsMapping");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMappings_SportsId",
                table: "AcademySportsMapping",
                newName: "IX_AcademySportsMapping_SportsId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMappings_last_modified_by",
                table: "AcademySportsMapping",
                newName: "IX_AcademySportsMapping_last_modified_by");

            migrationBuilder.RenameIndex(
                name: "IX_AcademySportsMappings_created_by",
                table: "AcademySportsMapping",
                newName: "IX_AcademySportsMapping_created_by");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademySportsMapping",
                table: "AcademySportsMapping",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportsId",
                table: "AcademySportsMapping",
                column: "SportsId",
                principalTable: "Sports",
                principalColumn: "id");
        }
    }
}
