using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VoiceRecordAPI.Migrations
{
    public partial class CreateVoiceRecord2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("232974b2-e3d0-4a6a-b1e6-d60b5bf56242"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5be0b8b7-09a0-4950-a702-9cf6ff0c8c3c"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7444a088-1ab2-4c96-af22-3dfeffb95cce"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("92ffbb62-3a06-45f7-bf4b-17a9ca3a89d5"));

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "VoiceRecordDetails");

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2a18809e-3875-4c41-95ae-678a4f4cf339"), "user" },
                    { new Guid("5457dd76-3c39-42c3-b543-8faf043e427b"), "Manager" },
                    { new Guid("842d6dc0-13e6-48cf-8ffb-db6295251dcd"), "Admin" },
                    { new Guid("f9c15ffa-d0c6-42f6-9509-25d0948a5885"), "Developer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2a18809e-3875-4c41-95ae-678a4f4cf339"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5457dd76-3c39-42c3-b543-8faf043e427b"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("842d6dc0-13e6-48cf-8ffb-db6295251dcd"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f9c15ffa-d0c6-42f6-9509-25d0948a5885"));

            migrationBuilder.AddColumn<float>(
                name: "FileSize",
                table: "VoiceRecordDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("92ffbb62-3a06-45f7-bf4b-17a9ca3a89d5"), "user" },
                    { new Guid("232974b2-e3d0-4a6a-b1e6-d60b5bf56242"), "Manager" },
                    { new Guid("5be0b8b7-09a0-4950-a702-9cf6ff0c8c3c"), "Admin" },
                    { new Guid("7444a088-1ab2-4c96-af22-3dfeffb95cce"), "Developer" }
                });
        }
    }
}
