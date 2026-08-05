using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CambiosConNuevoPepperSaltGeneracionHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "PasswordHash",
                value: "$2a$11$jU5w1nbtaHPBtejM6QNBX.oU8BJT/WRz0L1UreuWqxvFoamJHPOe.");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "PasswordHash",
                value: "$2a$11$TX24mECUOH4HvNjvCGc1NuW05lIVP/qZYhGSZeuBwVj22ngOmubym");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "PasswordHash",
                value: "$2a$11$txhrhOsQ4ktfXZ19Rc.H/.DkQ/pUVkSVsAu3WdHadlf32gpxBwG9O");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "PasswordHash",
                value: "$2a$11$9dxrNwWP8E9LpQAg7YbnCuJ/hI9oQlLzJmZm.bNe2Yq.EWFZiqAzG");
        }
    }
}
