using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VoiceRecordAPI.Migrations
{
    public partial class CreateVoiceRecord3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IncomingNo",
                table: "VoiceRecordDetails");

            migrationBuilder.DropColumn(
                name: "OutgoingNo",
                table: "VoiceRecordDetails");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberFrom",
                table: "VoiceRecordDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberTo",
                table: "VoiceRecordDetails",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("4db8696f-61c6-49a2-99c0-f8f3b969179a"), "user" },
                    { new Guid("837c4b89-c677-4fd0-9f4a-72ec62d2d216"), "Manager" },
                    { new Guid("4cb57245-8b10-4ce2-8170-c2a99b7f8880"), "Admin" },
                    { new Guid("660ee222-3eba-4175-93f3-b355b480bd8c"), "Developer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4cb57245-8b10-4ce2-8170-c2a99b7f8880"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4db8696f-61c6-49a2-99c0-f8f3b969179a"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("660ee222-3eba-4175-93f3-b355b480bd8c"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("837c4b89-c677-4fd0-9f4a-72ec62d2d216"));

            migrationBuilder.DropColumn(
                name: "PhoneNumberFrom",
                table: "VoiceRecordDetails");

            migrationBuilder.DropColumn(
                name: "PhoneNumberTo",
                table: "VoiceRecordDetails");

            migrationBuilder.AddColumn<string>(
                name: "IncomingNo",
                table: "VoiceRecordDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingNo",
                table: "VoiceRecordDetails",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
