using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class UpdateWithReviewer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewerName",
                table: "Review",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "292f321b-55f8-4dd7-aeef-29e57577511d", "AQAAAAEAACcQAAAAEGcNo6oPlCdd0U5vOd3IKtvJma8f+mESvq0tJWL2u0FETUvDrC/STn/bdDsbI7XZhw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewerName",
                table: "Review");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "14f3a8bb-5008-411c-853e-45a48eb7ae59", "AQAAAAEAACcQAAAAEMccZLrMpCQcAVioHpt5p3F08KmiuwBfe9mRHCQ6X/UAKWLxSxDjkASwhjp1WkrOGQ==" });
        }
    }
}
