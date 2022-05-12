using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class Poliza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polizas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificacionCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimientoCliente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPoliza = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoberturasCubiertas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorMaximoPoliza = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NombrePoliza = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreCiudadCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DireccionCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlacaAutoCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehiculoCuentaInspeccion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polizas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Polizas");
        }
    }
}
