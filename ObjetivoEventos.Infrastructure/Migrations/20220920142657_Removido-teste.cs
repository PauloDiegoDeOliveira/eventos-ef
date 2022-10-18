using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class Removidoteste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Teste",
                table: "Pedidos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Teste",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}