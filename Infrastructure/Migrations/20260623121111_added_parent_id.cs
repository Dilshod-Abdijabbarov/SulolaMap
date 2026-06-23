using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_parent_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "parent_id",
                table: "persons",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b1d4d10b-2f84-4f08-a4b7-2b38600aa64e"),
                columns: new[] { "ConcurrencyStamp", "LastActive", "PasswordHash" },
                values: new object[] { "099c7519-1f07-4225-a007-7cb95bbdae80", new DateTime(2026, 6, 23, 17, 11, 11, 264, DateTimeKind.Utc).AddTicks(8130), "AQAAAAIAAYagAAAAED2iLF6Mo4/hR3ioauFK1zwYyaZHG0g2iUx5NLfv9FMcfcpu5T+9shZeCnHHcpUsEA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "persons");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b1d4d10b-2f84-4f08-a4b7-2b38600aa64e"),
                columns: new[] { "ConcurrencyStamp", "LastActive", "PasswordHash" },
                values: new object[] { "6291c0fc-e0c3-4894-a00a-80a47856631e", new DateTime(2026, 6, 3, 7, 38, 25, 570, DateTimeKind.Utc).AddTicks(5136), "AQAAAAIAAYagAAAAELBgyT1tsCmGr1LTscTx9DJHvoQaJ2QooQ+BgdhHdnS7a8vkjjhcI3CgikPuhHuukQ==" });
        }
    }
}
