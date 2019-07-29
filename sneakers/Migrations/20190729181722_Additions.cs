using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class Additions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6e50f2cf-db31-4302-9725-3566e682a7b7", "AQAAAAEAACcQAAAAEG0tL3mSk5oWdBBccQGUjtpcKc6UC6Z4adIfGNyeKVlYpAUeXaFv2nEOkRLA2arKnA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e786e296-bce4-4daa-a744-672fd367e5df", "AQAAAAEAACcQAAAAED7z5RCHgGUnhAL6QLVrlasDsz7m5CcKtPrMwX/YmaGVpbIUxtpnYshshffA/GsT/A==" });
        }
    }
}
