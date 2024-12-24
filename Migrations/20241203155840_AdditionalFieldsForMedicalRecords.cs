using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalFieldsForMedicalRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ConsultantFee",
                table: "MedicalRecords",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DocComm",
                table: "MedicalRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Nurse1Comm",
                table: "MedicalRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Nurse1Id",
                table: "MedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Nurse2Comm",
                table: "MedicalRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Nurse2Id",
                table: "MedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_Nurse1Id",
                table: "MedicalRecords",
                column: "Nurse1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_Nurse2Id",
                table: "MedicalRecords",
                column: "Nurse2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_Nurse1Id",
                table: "MedicalRecords",
                column: "Nurse1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_Nurse2Id",
                table: "MedicalRecords",
                column: "Nurse2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_Nurse1Id",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_Nurse2Id",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_Nurse1Id",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_Nurse2Id",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ConsultantFee",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "DocComm",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Nurse1Comm",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Nurse1Id",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Nurse2Comm",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Nurse2Id",
                table: "MedicalRecords");
        }
    }
}
