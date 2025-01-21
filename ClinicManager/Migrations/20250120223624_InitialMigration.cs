using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        // Constantes para los literales duplicados
        private const string SqlServerIdentity = "SqlServer:Identity";
        private const string NVarCharMax = "nvarchar(max)";
        private const string CitasTable = "Citas";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    IdDoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation(SqlServerIdentity, "1, 1"),
                    Nombre = table.Column<string>(type: NVarCharMax, nullable: false),
                    Apellido = table.Column<string>(type: NVarCharMax, nullable: false),
                    Especialidad = table.Column<string>(type: NVarCharMax, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.IdDoctor);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation(SqlServerIdentity, "1, 1"),
                    Nombre = table.Column<string>(type: NVarCharMax, nullable: false),
                    Apellido = table.Column<string>(type: NVarCharMax, nullable: false),
                    Telefono = table.Column<string>(type: NVarCharMax, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                });

            migrationBuilder.CreateTable(
                name: CitasTable,
                columns: table => new
                {
                    IdCita = table.Column<int>(type: "int", nullable: false)
                        .Annotation(SqlServerIdentity, "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    IdDoctor = table.Column<int>(type: "int", nullable: false),
                    Lugar = table.Column<string>(type: NVarCharMax, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IdCita);
                    table.ForeignKey(
                        name: "FK_Citas_Doctores_IdDoctor",
                        column: x => x.IdDoctor,
                        principalTable: "Doctores",
                        principalColumn: "IdDoctor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procedimientos",
                columns: table => new
                {
                    IdProcedimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation(SqlServerIdentity, "1, 1"),
                    Descripcion = table.Column<string>(type: NVarCharMax, nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdCita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedimientos", x => x.IdProcedimiento);
                    table.ForeignKey(
                        name: "FK_Procedimientos_Citas_IdCita",
                        column: x => x.IdCita,
                        principalTable: CitasTable,
                        principalColumn: "IdCita",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdDoctor",
                table: CitasTable,
                column: "IdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IdPaciente",
                table: CitasTable,
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_Procedimientos_IdCita",
                table: "Procedimientos",
                column: "IdCita");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procedimientos");

            migrationBuilder.DropTable(
                name: CitasTable);

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
