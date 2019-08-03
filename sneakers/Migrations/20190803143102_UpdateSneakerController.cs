using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class UpdateSneakerController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f2475275-ccbd-4a98-bb47-efa8b32bb4fd", "AQAAAAEAACcQAAAAEDvbM3W94OvOuKg09hmLPQFPGtiNQehxzuqVAffub4DMTGhYe+Qf2TnOudvJfgYvEw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4e416e30-b3e9-4e1e-a66f-11c0524956e9", "AQAAAAEAACcQAAAAEK/jKZayV+pS47JHbU3A2FKfJ+Xq1Ot8mQNaAlLM9ZPq5sHb/NGyHM3JVbCRe0G8kw==" });
        }
    }
}
