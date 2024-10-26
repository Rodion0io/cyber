using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospital_api.Migrations
{
    /// <inheritdoc />
    public partial class Diagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
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
                name: "InspectionComment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    parentId = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<Guid>(type: "uuid", nullable: false),
                    modifyTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionComment", x => x.id);
                    table.ForeignKey(
                        name: "FK_InspectionComment_DoctorModel_author",
                        column: x => x.author,
                        principalTable: "DoctorModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionConsultation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    inspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    specialityid = table.Column<string>(type: "text", nullable: true),
                    rootComment = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionCommentModelid = table.Column<Guid>(type: "uuid", nullable: false),
                    commentsNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionConsultation", x => x.id);
                    table.ForeignKey(
                        name: "FK_InspectionConsultation_InspectionComment_InspectionCommentM~",
                        column: x => x.InspectionCommentModelid,
                        principalTable: "InspectionComment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionConsultation_Specialities_specialityid",
                        column: x => x.specialityid,
                        principalTable: "Specialities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Inspection",
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
                    doctor = table.Column<Guid>(type: "uuid", nullable: false),
                    diagnoses = table.Column<Guid>(type: "uuid", nullable: false),
                    consultations = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inspection_Diagnosis_diagnoses",
                        column: x => x.diagnoses,
                        principalTable: "Diagnosis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspection_DoctorModel_doctor",
                        column: x => x.doctor,
                        principalTable: "DoctorModel",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspection_InspectionConsultation_consultations",
                        column: x => x.consultations,
                        principalTable: "InspectionConsultation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspection_Patients_patient",
                        column: x => x.patient,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_consultations",
                table: "Inspection",
                column: "consultations");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_diagnoses",
                table: "Inspection",
                column: "diagnoses");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_doctor",
                table: "Inspection",
                column: "doctor");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_patient",
                table: "Inspection",
                column: "patient");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionComment_author",
                table: "InspectionComment",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionConsultation_InspectionCommentModelid",
                table: "InspectionConsultation",
                column: "InspectionCommentModelid");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionConsultation_specialityid",
                table: "InspectionConsultation",
                column: "specialityid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspection");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "InspectionConsultation");

            migrationBuilder.DropTable(
                name: "InspectionComment");

            migrationBuilder.DropTable(
                name: "DoctorModel");
        }
    }
}
