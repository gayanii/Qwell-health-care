using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldsAddedForLabRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRecords_Users_UserId",
                table: "LabRecords");

            migrationBuilder.DropIndex(
                name: "IX_LabRecords_UserId",
                table: "LabRecords");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LabRecords",
                newName: "AddedBy");

            migrationBuilder.AddColumn<float>(
                name: "ConsultantFee",
                table: "LabRecords",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DocComm",
                table: "LabRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "LabRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "LabBill",
                table: "LabRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Nurse1Comm",
                table: "LabRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Nurse1Id",
                table: "LabRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Nurse2Comm",
                table: "LabRecords",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Nurse2Id",
                table: "LabRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OtherCharges",
                table: "LabRecords",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_DoctorId",
                table: "LabRecords",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_Nurse1Id",
                table: "LabRecords",
                column: "Nurse1Id");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_Nurse2Id",
                table: "LabRecords",
                column: "Nurse2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRecords_Users_DoctorId",
                table: "LabRecords",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRecords_Users_Nurse1Id",
                table: "LabRecords",
                column: "Nurse1Id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRecords_Users_Nurse2Id",
                table: "LabRecords",
                column: "Nurse2Id",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabRecords_Users_DoctorId",
                table: "LabRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_LabRecords_Users_Nurse1Id",
                table: "LabRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_LabRecords_Users_Nurse2Id",
                table: "LabRecords");

            migrationBuilder.DropIndex(
                name: "IX_LabRecords_DoctorId",
                table: "LabRecords");

            migrationBuilder.DropIndex(
                name: "IX_LabRecords_Nurse1Id",
                table: "LabRecords");

            migrationBuilder.DropIndex(
                name: "IX_LabRecords_Nurse2Id",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "ConsultantFee",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "DocComm",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "LabBill",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "Nurse1Comm",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "Nurse1Id",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "Nurse2Comm",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "Nurse2Id",
                table: "LabRecords");

            migrationBuilder.DropColumn(
                name: "OtherCharges",
                table: "LabRecords");

            migrationBuilder.RenameColumn(
                name: "AddedBy",
                table: "LabRecords",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_UserId",
                table: "LabRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabRecords_Users_UserId",
                table: "LabRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
