using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class NumeroPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Numero",
                table: "Pedidos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Pedidos");
        }
    }
}