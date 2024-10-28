using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospital_api.Migrations
{
    /// <inheritdoc />
    public partial class inspectMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    inspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    specialityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Consultations_Specialities_specialityId",
                        column: x => x.specialityId,
                        principalTable: "Specialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    code = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    icdDiagnosisId = table.Column<Guid>(type: "uuid", nullable: false),
                    inspectionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosis", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    birthday = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorModel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    authorId = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    parentId = table.Column<Guid>(type: "uuid", nullable: false),
                    consultationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_Consultations_consultationId",
                        column: x => x.consultationId,
                        principalTable: "Consultations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    anamnesis = table.Column<string>(type: "text", nullable: false),
                    complaints = table.Column<string>(type: "text", nullable: false),
                    treatment = table.Column<string>(type: "text", nullable: false),
                    conclusion = table.Column<int>(type: "integer", nullable: false),
                    nextVisitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deathDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    baseInspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    previousInspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    patient = table.Column<Guid>(type: "uuid", nullable: false),
                    doctor = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inspections_DoctorModel_doctor",
                        column: x => x.doctor,
                        principalTable: "DoctorModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_Patients_patient",
                        column: x => x.patient,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_consultationId",
                table: "Comments",
                column: "consultationId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_specialityId",
                table: "Consultations",
                column: "specialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_doctor",
                table: "Inspections",
                column: "doctor");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_patient",
                table: "Inspections",
                column: "patient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "DoctorModel");
        }
    }
}
