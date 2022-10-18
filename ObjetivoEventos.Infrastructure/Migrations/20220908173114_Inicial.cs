using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace ObjetivoEventos.Infrastructure.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cadeiras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Fileira = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Coluna = table.Column<int>(type: "int", maxLength: 10000, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadeiras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CEP = table.Column<string>(type: "varchar(10)", nullable: false),
                    Estado = table.Column<string>(type: "varchar(100)", nullable: false),
                    Cidade = table.Column<string>(type: "varchar(100)", nullable: false),
                    Logradouro = table.Column<string>(type: "varchar(100)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Complemento = table.Column<string>(type: "varchar(100)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Telefone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CEP = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Alias = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Fileira = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Coluna = table.Column<int>(type: "int", maxLength: 10000, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorTotal = table.Column<float>(type: "real", nullable: false),
                    SituacaoPagamento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "AguardandoPagamento"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Sobre = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    DataEvento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cantor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NomeArquivoBanco = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TamanhoEmBytes = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensaoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeArquivoOriginal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaminhoRelativo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaminhoAbsoluto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaminhoFisico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraEnvio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Locais_LocalId",
                        column: x => x.LocalId,
                        principalTable: "Locais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Preco = table.Column<float>(type: "real", maxLength: 10000, nullable: false),
                    Posicao = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Alias = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Setores_Locais_LocalId",
                        column: x => x.LocalId,
                        principalTable: "Locais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CadeiraMesa",
                columns: table => new
                {
                    CadeirasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MesasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadeiraMesa", x => new { x.CadeirasId, x.MesasId });
                    table.ForeignKey(
                        name: "FK_CadeiraMesa_Cadeiras_CadeirasId",
                        column: x => x.CadeirasId,
                        principalTable: "Cadeiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadeiraMesa_Mesas_MesasId",
                        column: x => x.MesasId,
                        principalTable: "Mesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalId = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    SetorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MesaId = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true),
                    CadeiraId = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Situacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Reservado"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CadeiraSetor",
                columns: table => new
                {
                    CadeirasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetoresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadeiraSetor", x => new { x.CadeirasId, x.SetoresId });
                    table.ForeignKey(
                        name: "FK_CadeiraSetor_Cadeiras_CadeirasId",
                        column: x => x.CadeirasId,
                        principalTable: "Cadeiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadeiraSetor_Setores_SetoresId",
                        column: x => x.SetoresId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MesaSetor",
                columns: table => new
                {
                    MesasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetoresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MesaSetor", x => new { x.MesasId, x.SetoresId });
                    table.ForeignKey(
                        name: "FK_MesaSetor_Mesas_MesasId",
                        column: x => x.MesasId,
                        principalTable: "Mesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MesaSetor_Setores_SetoresId",
                        column: x => x.SetoresId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoReserva",
                columns: table => new
                {
                    PedidosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoReserva", x => new { x.PedidosId, x.ReservasId });
                    table.ForeignKey(
                        name: "FK_PedidoReserva_Pedidos_PedidosId",
                        column: x => x.PedidosId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoReserva_Reservas_ReservasId",
                        column: x => x.ReservasId,
                        principalTable: "Reservas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CadeiraMesa_MesasId",
                table: "CadeiraMesa",
                column: "MesasId");

            migrationBuilder.CreateIndex(
                name: "IX_CadeiraSetor_SetoresId",
                table: "CadeiraSetor",
                column: "SetoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_LocalId",
                table: "Eventos",
                column: "LocalId");

            migrationBuilder.CreateIndex(
                name: "IX_MesaSetor_SetoresId",
                table: "MesaSetor",
                column: "SetoresId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoReserva_ReservasId",
                table: "PedidoReserva",
                column: "ReservasId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EventoId",
                table: "Reservas",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Setores_LocalId",
                table: "Setores",
                column: "LocalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CadeiraMesa");

            migrationBuilder.DropTable(
                name: "CadeiraSetor");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "MesaSetor");

            migrationBuilder.DropTable(
                name: "PedidoReserva");

            migrationBuilder.DropTable(
                name: "Cadeiras");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Locais");
        }
    }
}