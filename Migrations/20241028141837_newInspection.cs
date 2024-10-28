using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hospital_api.Migrations
{
    /// <inheritdoc />
    public partial class newInspection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_DoctorModel_doctor",
                table: "Inspections");

            migrationBuilder.DropTable(
                name: "DoctorModel");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Doctors_doctor",
                table: "Inspections",
                column: "doctor",
                principalTable: "Doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Doctors_doctor",
                table: "Inspections");

            migrationBuilder.CreateTable(
                name: "DoctorModel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    birthday = table.Column<string>(type: "text", nullable: true),
                    createTime = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorModel", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_DoctorModel_doctor",
                table: "Inspections",
                column: "doctor",
                principalTable: "DoctorModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
