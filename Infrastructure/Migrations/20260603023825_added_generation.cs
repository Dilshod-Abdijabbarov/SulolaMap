using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_generation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "order",
                table: "persons",
                newName: "child_order");

            migrationBuilder.AddColumn<Guid>(
                name: "generation_id",
                table: "persons",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "generations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generations", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b1d4d10b-2f84-4f08-a4b7-2b38600aa64e"),
                columns: new[] { "ConcurrencyStamp", "LastActive", "PasswordHash" },
                values: new object[] { "6291c0fc-e0c3-4894-a00a-80a47856631e", new DateTime(2026, 6, 3, 7, 38, 25, 570, DateTimeKind.Utc).AddTicks(5136), "AQAAAAIAAYagAAAAELBgyT1tsCmGr1LTscTx9DJHvoQaJ2QooQ+BgdhHdnS7a8vkjjhcI3CgikPuhHuukQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_persons_generation_id",
                table: "persons",
                column: "generation_id");

            migrationBuilder.AddForeignKey(
                name: "FK_persons_generations_generation_id",
                table: "persons",
                column: "generation_id",
                principalTable: "generations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_generations_generation_id",
                table: "persons");

            migrationBuilder.DropTable(
                name: "generations");

            migrationBuilder.DropIndex(
                name: "IX_persons_generation_id",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "generation_id",
                table: "persons");

            migrationBuilder.RenameColumn(
                name: "child_order",
                table: "persons",
                newName: "order");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b1d4d10b-2f84-4f08-a4b7-2b38600aa64e"),
                columns: new[] { "ConcurrencyStamp", "LastActive", "PasswordHash" },
                values: new object[] { "75eed0b4-2156-414c-a85f-7c775d8bd9a5", new DateTime(2026, 4, 12, 17, 18, 36, 40, DateTimeKind.Utc).AddTicks(1608), "AQAAAAIAAYagAAAAEAPHBwBiqwQWMjoCMCxmw93L3TNwbR8rVWdnVUfgk6siDF67WCdVPPeNf4T0fuvlcA==" });
        }
    }
}
