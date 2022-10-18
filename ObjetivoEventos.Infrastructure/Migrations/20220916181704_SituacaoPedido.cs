using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class SituacaoPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SituacaoPagamento",
                table: "Pedidos",
                newName: "SituacaoPedido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SituacaoPedido",
                table: "Pedidos",
                newName: "SituacaoPagamento");
        }
    }
}