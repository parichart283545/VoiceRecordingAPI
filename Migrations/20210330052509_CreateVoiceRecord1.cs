using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VoiceRecordAPI.Migrations
{
    public partial class CreateVoiceRecord1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "CallType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoiceRecordConfigurations",
                columns: table => new
                {
                    ParameterName = table.Column<string>(nullable: false),
                    ValueNumber = table.Column<float>(nullable: false),
                    ValueString = table.Column<string>(nullable: true),
                    ValueDatetime = table.Column<DateTime>(nullable: false),
                    ValueBoolean = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceRecordConfigurations", x => x.ParameterName);
                });

            migrationBuilder.CreateTable(
                name: "VoiceRecordProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detail = table.Column<string>(maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceRecordProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoiceRecordDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtensionNo = table.Column<int>(nullable: false),
                    PhoneNumberFrom = table.Column<string>(nullable: true),
                    PhoneNumberTo = table.Column<string>(nullable: true),
                    DatetimeFileName = table.Column<DateTime>(nullable: false),
                    FileCreateDatetime = table.Column<DateTime>(nullable: false),
                    FileModifyDatetime = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(maxLength: 4096, nullable: true),
                    FilePath = table.Column<string>(maxLength: 4096, nullable: true),
                    FullPath = table.Column<string>(maxLength: 4096, nullable: true),
                    URLPath = table.Column<string>(maxLength: 4096, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    VoiceRecordProvidersId = table.Column<int>(nullable: false),
                    CallTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceRecordDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceRecordDetails_CallType_CallTypeId",
                        column: x => x.CallTypeId,
                        principalTable: "CallType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoiceRecordDetails_VoiceRecordProviders_VoiceRecordProvidersId",
                        column: x => x.VoiceRecordProvidersId,
                        principalTable: "VoiceRecordProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "auth",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoiceRecordURLRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VoiceRecordDetailId = table.Column<int>(nullable: false),
                    VoiceRecordDetailURL = table.Column<string>(maxLength: 4096, nullable: true),
                    ResponseURL = table.Column<string>(maxLength: 4096, nullable: true),
                    CreatedDatetime = table.Column<DateTime>(nullable: false),
                    ExpireDatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceRecordURLRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceRecordURLRequest_VoiceRecordDetails_VoiceRecordDetailId",
                        column: x => x.VoiceRecordDetailId,
                        principalTable: "VoiceRecordDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CallType",
                columns: new[] { "Id", "Detail", "IsActive", "Remark" },
                values: new object[,]
                {
                    { 1, "Inbound", true, "Inbound calling" },
                    { 2, "Outbound", true, "Outbound calling" },
                    { 3, "Unknow", true, "Unknow" }
                });

            migrationBuilder.InsertData(
                table: "VoiceRecordConfigurations",
                columns: new[] { "ParameterName", "Remark", "ValueBoolean", "ValueDatetime", "ValueNumber", "ValueString" },
                values: new object[,]
                {
                    { "WebDomainFormat", "Web client format for media player client", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "Http://xxxxxx/{GUID}" },
                    { "URLTimeout", "ValueString is unit name (minute,hour,day,month,year) ,ValueNumber is value ", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7f, "Day" },
                    { "EricssonFileFormatCallIn", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "DateTime-Extentions" },
                    { "EricssonFileFormatCallOut", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "DateTime-Extentions" },
                    { "EricssonPathFormat", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "" },
                    { "EricssonPath", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "" },
                    { "ThreeCXFileFormatCallIn", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "[Extension%3ACalledNumber]_CalledNumber-Extension_YearMonthDayHourMinuteSecond(InternalCallIdentifier)" },
                    { "ThreeCXFileFormatCallOut", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "[DisplayName]_Extension-CalledNumber_YearMonthDayHourMinuteSecond(InternalCallIdentifier)" },
                    { "ThreeCXPathFormat", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "" },
                    { "ThreeCXPath", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0f, "" }
                });

            migrationBuilder.InsertData(
                table: "VoiceRecordProviders",
                columns: new[] { "Id", "Detail", "IsActive", "Remark" },
                values: new object[,]
                {
                    { 1, "3CX", true, "3CX" },
                    { 2, "Ericsson", true, "Ericsson" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("fd1dc1d4-ce08-475a-95df-2bab9a0d0a82"), "Developer" },
                    { new Guid("4205fa1c-00a9-4978-8948-995d7aba7862"), "Admin" },
                    { new Guid("27ff4c51-f0ca-4b67-8513-4879cb13b0f3"), "Manager" },
                    { new Guid("c20e34f7-3fc4-4e65-8c2b-b1b8d2ba2cfc"), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceRecordDetails_CallTypeId",
                table: "VoiceRecordDetails",
                column: "CallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceRecordDetails_VoiceRecordProvidersId",
                table: "VoiceRecordDetails",
                column: "VoiceRecordProvidersId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceRecordURLRequest_VoiceRecordDetailId",
                table: "VoiceRecordURLRequest",
                column: "VoiceRecordDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "auth",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceRecordConfigurations");

            migrationBuilder.DropTable(
                name: "VoiceRecordURLRequest");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "VoiceRecordDetails");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "User",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "CallType");

            migrationBuilder.DropTable(
                name: "VoiceRecordProviders");
        }
    }
}
