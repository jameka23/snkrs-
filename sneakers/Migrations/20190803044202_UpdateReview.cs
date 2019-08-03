using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class UpdateReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewMessage",
                table: "Review",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "14f3a8bb-5008-411c-853e-45a48eb7ae59", "AQAAAAEAACcQAAAAEMccZLrMpCQcAVioHpt5p3F08KmiuwBfe9mRHCQ6X/UAKWLxSxDjkASwhjp1WkrOGQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewMessage",
                table: "Review");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c1ec41d6-976c-484b-bcb2-ab987bd7fd76", "AQAAAAEAACcQAAAAEEceEG1/J10dcCvzRaZuRMqmds2LR8TGW6H+qsHHF/JyO8cLXJlanYPI0t3wzc/3KA==" });
        }
    }
}
