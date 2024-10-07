using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class addSubscriptionIdField_UserInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "UserInfo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

           
            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_Subscription_SubscriptionId",
                table: "UserInfo",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfo_Subscription_SubscriptionId",
                table: "UserInfo");

          

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "UserInfo");
        }
    }
}
