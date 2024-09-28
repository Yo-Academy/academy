using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class Add_Column_IsActive_academy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Academies_AcademyId",
                table: "AcademySportsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportId",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_AcademyId",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_SportId",
                table: "AcademySportsMapping");

            migrationBuilder.AddColumn<Guid>(
                name: "SportsId",
                table: "AcademySportsMapping",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "Academies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Academies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GST",
                table: "Academies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Academies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Subdomain",
                table: "Academies",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_SportsId",
                table: "AcademySportsMapping",
                column: "SportsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportsId",
                table: "AcademySportsMapping",
                column: "SportsId",
                principalTable: "Sports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportsId",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_SportsId",
                table: "AcademySportsMapping");

            migrationBuilder.DropColumn(
                name: "SportsId",
                table: "AcademySportsMapping");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Academies");

            migrationBuilder.DropColumn(
                name: "Subdomain",
                table: "Academies");

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "Academies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Academies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GST",
                table: "Academies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_AcademyId",
                table: "AcademySportsMapping",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_SportId",
                table: "AcademySportsMapping",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMapping_Academies_AcademyId",
                table: "AcademySportsMapping",
                column: "AcademyId",
                principalTable: "Academies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademySportsMapping_Sports_SportId",
                table: "AcademySportsMapping",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
