using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class AnotherOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4e416e30-b3e9-4e1e-a66f-11c0524956e9", "AQAAAAEAACcQAAAAEK/jKZayV+pS47JHbU3A2FKfJ+Xq1Ot8mQNaAlLM9ZPq5sHb/NGyHM3JVbCRe0G8kw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "292f321b-55f8-4dd7-aeef-29e57577511d", "AQAAAAEAACcQAAAAEGcNo6oPlCdd0U5vOd3IKtvJma8f+mESvq0tJWL2u0FETUvDrC/STn/bdDsbI7XZhw==" });
        }
    }
}
