using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QWellApp.Migrations
{
    /// <inheritdoc />
    public partial class newChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    LabPaid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllergicHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Generic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentQuantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    CommissionCharge = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commissions_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ChitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalBill = table.Column<float>(type: "real", nullable: false),
                    TotalLabPaidCost = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
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
                    AddedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Nurse1Id = table.Column<int>(type: "int", nullable: false),
                    Nurse2Id = table.Column<int>(type: "int", nullable: false),
                    ChitNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OPDCharge = table.Column<float>(type: "real", nullable: true),
                    ConsultantFee = table.Column<float>(type: "real", nullable: true),
                    OtherCharges = table.Column<float>(type: "real", nullable: true),
                    ProcedureBill = table.Column<float>(type: "real", nullable: false),
                    AdmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalBill = table.Column<float>(type: "real", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    DocComm = table.Column<float>(type: "real", nullable: true),
                    Nurse1Comm = table.Column<float>(type: "real", nullable: true),
                    Nurse2Comm = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureRecords_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureRecords_Users_Nurse1Id",
                        column: x => x.Nurse1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureRecords_Users_Nurse2Id",
                        column: x => x.Nurse2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SupplierPrice = table.Column<float>(type: "real", nullable: false),
                    SellingPrice = table.Column<float>(type: "real", nullable: false),
                    OrderedQuantity = table.Column<int>(type: "int", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecords_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabRecordTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestId = table.Column<int>(type: "int", nullable: false),
                    LabRecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabRecordTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabRecordTests_LabRecords_LabRecordId",
                        column: x => x.LabRecordId,
                        principalTable: "LabRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabRecordTests_LabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    SoldPrice = table.Column<float>(type: "real", nullable: false),
                    RecordTypeId = table.Column<int>(type: "int", nullable: false),
                    RecordTypeId1 = table.Column<int>(type: "int", nullable: false),
                    RecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMedicalRecords_LabRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "LabRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMedicalRecords_MedicalRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMedicalRecords_ProcedureRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "ProcedureRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMedicalRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMedicalRecords_Records_RecordTypeId1",
                        column: x => x.RecordTypeId1,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1, "Medical" },
                    { 2, "Lab" },
                    { 3, "Procedure" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Doctor" },
                    { 2, "Nursing Assistant" },
                    { 3, "Intern Nurse" },
                    { 4, "Junior Nurse" },
                    { 5, "Nursing Officer" },
                    { 6, "Senior Nurse" },
                    { 7, "Chief Nurse" },
                    { 8, "Nursing Supervisor" },
                    { 9, "Operation Manager" },
                    { 10, "Manager" },
                    { 11, "Director" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_DoctorId",
                table: "Commissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_PatientId",
                table: "LabRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecords_UserId",
                table: "LabRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecordTests_LabRecordId",
                table: "LabRecordTests",
                column: "LabRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_LabRecordTests_LabTestId",
                table: "LabRecordTests",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRecords_DoctorId",
                table: "ProcedureRecords",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRecords_Nurse1Id",
                table: "ProcedureRecords",
                column: "Nurse1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRecords_Nurse2Id",
                table: "ProcedureRecords",
                column: "Nurse2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRecords_PatientId",
                table: "ProcedureRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_ProductId",
                table: "ProductMedicalRecords",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_RecordId",
                table: "ProductMedicalRecords",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedicalRecords_RecordTypeId1",
                table: "ProductMedicalRecords",
                column: "RecordTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecords_ProductId",
                table: "ProductRecords",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecords_SupplierId",
                table: "ProductRecords",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecords_UserId",
                table: "ProductRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "LabRecordTests");

            migrationBuilder.DropTable(
                name: "ProductMedicalRecords");

            migrationBuilder.DropTable(
                name: "ProductRecords");

            migrationBuilder.DropTable(
                name: "LabTests");

            migrationBuilder.DropTable(
                name: "LabRecords");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "ProcedureRecords");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
