using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class recordIdsAddedSeperately : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.AddColumn<int>(
                name: "LabRecordId",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordId",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcedureRecordId",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_LabRecordId",
                table: "ProductMedicalRecords",
                column: "LabRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_MedicalRecordId",
                table: "ProductMedicalRecords",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_ProcedureRecordId",
                table: "ProductMedicalRecords",
                column: "ProcedureRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_LabRecords_LabRecordId",
                table: "ProductMedicalRecords",
                column: "LabRecordId",
                principalTable: "LabRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_MedicalRecords_MedicalRecordId",
                table: "ProductMedicalRecords",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_ProcedureRecords_ProcedureRecordId",
                table: "ProductMedicalRecords",
                column: "ProcedureRecordId",
                principalTable: "ProcedureRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_LabRecords_LabRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_MedicalRecords_MedicalRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_ProcedureRecords_ProcedureRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_LabRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_MedicalRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_ProcedureRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropColumn(
                name: "LabRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropColumn(
                name: "MedicalRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropColumn(
                name: "ProcedureRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.AddColumn<int>(
                name: "RecordId",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
