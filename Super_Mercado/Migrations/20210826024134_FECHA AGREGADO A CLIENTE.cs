using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Super_Mercado.Migrations
{
    public partial class FECHAAGREGADOACLIENTE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
     

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Usuarios",
                type: "DATE",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ImagenesWebPages",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Usuarios");

     
            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "ImagenesWebPages",
                type: "NVARCHAR(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
