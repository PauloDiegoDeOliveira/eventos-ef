using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class Teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Teste",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Teste",
                table: "Pedidos");
        }
    }
}