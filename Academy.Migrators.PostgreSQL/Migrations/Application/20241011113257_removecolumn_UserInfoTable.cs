using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class removecolumn_UserInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfo_PlanType_PlanTypeId",
                table: "UserInfo");

           

            migrationBuilder.DropColumn(
                name: "PlanTypeId",
                table: "UserInfo");

            migrationBuilder.RenameColumn(
                name: "IsACtive",
                table: "UserInfo",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "UserInfo",
                newName: "IsACtive");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanTypeId",
                table: "UserInfo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

           

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_PlanType_PlanTypeId",
                table: "UserInfo",
                column: "PlanTypeId",
                principalTable: "PlanType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
