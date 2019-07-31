using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class ErrorToldMe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a19455ee-489b-4856-946b-c30f0686cdc0", "AQAAAAEAACcQAAAAECg+Z6JC4M/DrZxF5ZAFtrG+HmZkJ2iywApVdzGt8JDNGKN0gVSIMbvMhcqc8n/vLA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6e50f2cf-db31-4302-9725-3566e682a7b7", "AQAAAAEAACcQAAAAEG0tL3mSk5oWdBBccQGUjtpcKc6UC6Z4adIfGNyeKVlYpAUeXaFv2nEOkRLA2arKnA==" });
        }
    }
}
