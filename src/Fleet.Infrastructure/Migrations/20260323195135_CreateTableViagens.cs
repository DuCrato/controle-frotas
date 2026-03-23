using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableViagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Viagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CondutorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrigemLatitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    OrigemLongitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    OrigemEndereco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DestinoLatitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    DestinoLongitude = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: false),
                    DestinoEndereco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataHoraPrevistaSaida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraPrevistaChegada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraRealSaida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataHoraRealChegada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuiliometragemInicial = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    QuiliometragemFinal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    DistanciaEstimada = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_CondutorId",
                table: "Viagens",
                column: "CondutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_VeiculoId",
                table: "Viagens",
                column: "VeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_Status",
                table: "Viagens",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_DataCriacao",
                table: "Viagens",
                column: "DataCriacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Viagens");
        }
    }
}
