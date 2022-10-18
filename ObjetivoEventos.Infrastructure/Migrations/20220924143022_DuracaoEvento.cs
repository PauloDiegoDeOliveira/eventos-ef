using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class DuracaoEvento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duracao",
                table: "Eventos",
                type: "int",
                maxLength: 10000,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duracao",
                table: "Eventos");
        }
    }
}