using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class AddChannelRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelRecordId",
                table: "ProductMedicalRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChannelRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ChitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OPDCharge = table.Column<float>(type: "real", nullable: true),
                    OtherCharges = table.Column<float>(type: "real", nullable: true),
                    PharmacyBill = table.Column<float>(type: "real", nullable: false),
                    AdmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalBill = table.Column<float>(type: "real", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    Nurse1Id = table.Column<int>(type: "int", nullable: true),
                    Nurse2Id = table.Column<int>(type: "int", nullable: true),
                    DocComm = table.Column<float>(type: "real", nullable: false),
                    Nurse1Comm = table.Column<float>(type: "real", nullable: false),
                    Nurse2Comm = table.Column<float>(type: "real", nullable: false),
                    ConsultantFee = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRecords_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRecords_Users_Nurse1Id",
                        column: x => x.Nurse1Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChannelRecords_Users_Nurse2Id",
                        column: x => x.Nurse2Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "Id", "TypeName" },
                values: new object[] { 4, "Channel" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_ChannelRecordId",
                table: "ProductMedicalRecords",
                column: "ChannelRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecords_DoctorId",
                table: "ChannelRecords",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecords_Nurse1Id",
                table: "ChannelRecords",
                column: "Nurse1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecords_Nurse2Id",
                table: "ChannelRecords",
                column: "Nurse2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecords_PatientId",
                table: "ChannelRecords",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedicalRecords_ChannelRecords_ChannelRecordId",
                table: "ProductMedicalRecords",
                column: "ChannelRecordId",
                principalTable: "ChannelRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedicalRecords_ChannelRecords_ChannelRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DropTable(
                name: "ChannelRecords");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedicalRecords_ChannelRecordId",
                table: "ProductMedicalRecords");

            migrationBuilder.DeleteData(
                table: "Records",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ChannelRecordId",
                table: "ProductMedicalRecords");
        }
    }
}
