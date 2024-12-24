using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class EnumChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_Records_RecordTypeId1",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_RecordTypeId1",
                table: "ProductMedicalRecords");

            migrationBuilder.DropColumn(
                name: "RecordTypeId1",
                table: "ProductMedicalRecords");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_RecordTypeId",
                table: "ProductMedicalRecords",
                column: "RecordTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_Records_RecordTypeId",
                table: "ProductMedicalRecords",
                column: "RecordTypeId",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_Records_RecordTypeId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_RecordTypeId",
                table: "ProductMedicalRecords");

            migrationBuilder.AddColumn<int>(
                name: "RecordTypeId1",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_RecordTypeId1",
                table: "ProductMedicalRecords",
                column: "RecordTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_Records_RecordTypeId1",
                table: "ProductMedicalRecords",
                column: "RecordTypeId1",
                principalTable: "Records",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
