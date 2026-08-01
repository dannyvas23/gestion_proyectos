using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Agg_Usuarios_Columas_Tareas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Columna_proyectos_ProyectoId",
                table: "Columna");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarea_Columna_ColumnaId",
                table: "Tarea");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarea_Usuario_ResponsableId",
                table: "Tarea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tarea",
                table: "Tarea");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Columna",
                table: "Columna");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "Tarea",
                newName: "tareas");

            migrationBuilder.RenameTable(
                name: "Columna",
                newName: "columnas");

            migrationBuilder.RenameIndex(
                name: "IX_Tarea_ResponsableId",
                table: "tareas",
                newName: "IX_tareas_ResponsableId");

            migrationBuilder.RenameIndex(
                name: "IX_Tarea_ColumnaId",
                table: "tareas",
                newName: "IX_tareas_ColumnaId");

            migrationBuilder.RenameIndex(
                name: "IX_Columna_ProyectoId",
                table: "columnas",
                newName: "IX_columnas_ProyectoId");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CorreoElectronico",
                table: "usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "tareas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Prioridad",
                table: "tareas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "tareas",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "columnas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "Activa",
                table: "columnas",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tareas",
                table: "tareas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_columnas",
                table: "columnas",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "PasswordHash", "Rol" },
                values: new object[] { "$2a$11$txhrhOsQ4ktfXZ19Rc.H/.DkQ/pUVkSVsAu3WdHadlf32gpxBwG9O", "Administrador" });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                columns: new[] { "PasswordHash", "Rol" },
                values: new object[] { "$2a$11$9dxrNwWP8E9LpQAg7YbnCuJ/hI9oQlLzJmZm.bNe2Yq.EWFZiqAzG", "Miembro" });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_CorreoElectronico",
                table: "usuarios",
                column: "CorreoElectronico",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_columnas_proyectos_ProyectoId",
                table: "columnas",
                column: "ProyectoId",
                principalTable: "proyectos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tareas_columnas_ColumnaId",
                table: "tareas",
                column: "ColumnaId",
                principalTable: "columnas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tareas_usuarios_ResponsableId",
                table: "tareas",
                column: "ResponsableId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_columnas_proyectos_ProyectoId",
                table: "columnas");

            migrationBuilder.DropForeignKey(
                name: "FK_tareas_columnas_ColumnaId",
                table: "tareas");

            migrationBuilder.DropForeignKey(
                name: "FK_tareas_usuarios_ResponsableId",
                table: "tareas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_CorreoElectronico",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tareas",
                table: "tareas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_columnas",
                table: "columnas");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuario");

            migrationBuilder.RenameTable(
                name: "tareas",
                newName: "Tarea");

            migrationBuilder.RenameTable(
                name: "columnas",
                newName: "Columna");

            migrationBuilder.RenameIndex(
                name: "IX_tareas_ResponsableId",
                table: "Tarea",
                newName: "IX_Tarea_ResponsableId");

            migrationBuilder.RenameIndex(
                name: "IX_tareas_ColumnaId",
                table: "Tarea",
                newName: "IX_Tarea_ColumnaId");

            migrationBuilder.RenameIndex(
                name: "IX_columnas_ProyectoId",
                table: "Columna",
                newName: "IX_Columna_ProyectoId");

            migrationBuilder.AlterColumn<int>(
                name: "Rol",
                table: "Usuario",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoElectronico",
                table: "Usuario",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Usuario",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Tarea",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Prioridad",
                table: "Tarea",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tarea",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Columna",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "Activa",
                table: "Columna",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tarea",
                table: "Tarea",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Columna",
                table: "Columna",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "PasswordHash", "Rol" },
                values: new object[] { "$2a$11$zXFOf4l2thtWyxhmcsnDT.wu0iCrCytHQWLz9b15x/n61EWDPuvVS", 1 });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                columns: new[] { "PasswordHash", "Rol" },
                values: new object[] { "$2a$11$qq0VvS9VOe9LZ2f1uzi8yOheV2XUoWCiVA2OjGJ3As0dS2La/HiES", 2 });

            migrationBuilder.AddForeignKey(
                name: "FK_Columna_proyectos_ProyectoId",
                table: "Columna",
                column: "ProyectoId",
                principalTable: "proyectos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarea_Columna_ColumnaId",
                table: "Tarea",
                column: "ColumnaId",
                principalTable: "Columna",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarea_Usuario_ResponsableId",
                table: "Tarea",
                column: "ResponsableId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
