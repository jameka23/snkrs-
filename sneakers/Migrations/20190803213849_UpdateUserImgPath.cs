using Microsoft.EntityFrameworkCore.Migrations;

namespace sneakers.Migrations
{
    public partial class UpdateUserImgPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4bc76278-dd70-4b59-9a23-ed11fea59c52", "AQAAAAEAACcQAAAAEPDY42GVk4BAVYEiM4JwUpaNi6FFOh86Vp+m7NQMawafzfg2Z1RV0NlQ+lgnfSTacA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000001-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f2475275-ccbd-4a98-bb47-efa8b32bb4fd", "AQAAAAEAACcQAAAAEDvbM3W94OvOuKg09hmLPQFPGtiNQehxzuqVAffub4DMTGhYe+Qf2TnOudvJfgYvEw==" });
        }
    }
}
