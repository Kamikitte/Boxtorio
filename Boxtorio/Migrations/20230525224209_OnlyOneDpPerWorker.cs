using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boxtorio.Migrations
{
    /// <inheritdoc />
    public partial class OnlyOneDpPerWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPointWorker");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryPointId",
                table: "Workers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DeliveryPointId",
                table: "Workers",
                column: "DeliveryPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_DeliveryPoints_DeliveryPointId",
                table: "Workers",
                column: "DeliveryPointId",
                principalTable: "DeliveryPoints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_DeliveryPoints_DeliveryPointId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_DeliveryPointId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "DeliveryPointId",
                table: "Workers");

            migrationBuilder.CreateTable(
                name: "DeliveryPointWorker",
                columns: table => new
                {
                    DeliveryPointsId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPointWorker", x => new { x.DeliveryPointsId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_DeliveryPointWorker_DeliveryPoints_DeliveryPointsId",
                        column: x => x.DeliveryPointsId,
                        principalTable: "DeliveryPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryPointWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPointWorker_WorkersId",
                table: "DeliveryPointWorker",
                column: "WorkersId");
        }
    }
}
