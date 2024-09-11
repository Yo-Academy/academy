using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrators.PostgreSQL.Migrations.Application
{
    /// <inheritdoc />
    public partial class Adding_foreign_key_AuditableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscription_created_by",
                table: "Subscription",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_last_modified_by",
                table: "Subscription",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_created_by",
                table: "Sports",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_last_modified_by",
                table: "Sports",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_created_by",
                table: "PlanType",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_PlanType_last_modified_by",
                table: "PlanType",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_Coaching_created_by",
                table: "Coaching",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Coaching_last_modified_by",
                table: "Coaching",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_created_by",
                table: "Batch",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Batch_last_modified_by",
                table: "Batch",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_created_by",
                table: "AcademySportsMapping",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_AcademySportsMapping_last_modified_by",
                table: "AcademySportsMapping",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "IX_Academies_created_by",
                table: "Academies",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Academies_last_modified_by",
                table: "Academies",
                column: "last_modified_by");

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Academies",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Academies",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "AcademySportsMapping",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "AcademySportsMapping",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Batch",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Batch",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Coaching",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Coaching",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "PlanType",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "PlanType",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Sports",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Sports",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Subscription",
                column: "created_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Subscription",
                column: "last_modified_by",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Academies");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Academies");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "AcademySportsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "AcademySportsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Batch");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Batch");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Coaching");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Coaching");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "PlanType");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "PlanType");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Sports");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Sports");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_CreatedBy",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Entity_ApplicationUser_LastModifiedBy",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_created_by",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_last_modified_by",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Sports_created_by",
                table: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_Sports_last_modified_by",
                table: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_PlanType_created_by",
                table: "PlanType");

            migrationBuilder.DropIndex(
                name: "IX_PlanType_last_modified_by",
                table: "PlanType");

            migrationBuilder.DropIndex(
                name: "IX_Coaching_created_by",
                table: "Coaching");

            migrationBuilder.DropIndex(
                name: "IX_Coaching_last_modified_by",
                table: "Coaching");

            migrationBuilder.DropIndex(
                name: "IX_Batch_created_by",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_Batch_last_modified_by",
                table: "Batch");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_created_by",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_AcademySportsMapping_last_modified_by",
                table: "AcademySportsMapping");

            migrationBuilder.DropIndex(
                name: "IX_Academies_created_by",
                table: "Academies");

            migrationBuilder.DropIndex(
                name: "IX_Academies_last_modified_by",
                table: "Academies");
        }
    }
}
