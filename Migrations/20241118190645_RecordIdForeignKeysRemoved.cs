using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class RecordIdForeignKeysRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_LabRecords_RecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_MedicalRecords_RecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_ProcedureRecords_RecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_RecordId",
                table: "ProductMedicalRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_RecordId",
                table: "ProductMedicalRecords",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_LabRecords_RecordId",
                table: "ProductMedicalRecords",
                column: "RecordId",
                principalTable: "LabRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_MedicalRecords_RecordId",
                table: "ProductMedicalRecords",
                column: "RecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_ProcedureRecords_RecordId",
                table: "ProductMedicalRecords",
                column: "RecordId",
                principalTable: "ProcedureRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
