using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ebyteLearner.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CourseName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoursePrice = table.Column<float>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SessionMonitoring",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ShowingResult = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShowingQuestion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ShowingQrCode = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Slide = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionMonitoring", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuleDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Module_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePhoto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NIF = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nationality = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRole = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Birthday = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pdf",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PDFName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PDFNumberPages = table.Column<int>(type: "int", nullable: false),
                    PDFContent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PDFLength = table.Column<long>(type: "bigint", nullable: false),
                    ModuleID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pdf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pdf_Module_ModuleID",
                        column: x => x.ModuleID,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionDescription = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QRCode = table.Column<byte[]>(type: "longblob", nullable: false),
                    PdfDocumentID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ModuleID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionMonitoringID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StartSessionDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    EndSessionDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Module_ModuleID",
                        column: x => x.ModuleID,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_Pdf_PdfDocumentID",
                        column: x => x.PdfDocumentID,
                        principalTable: "Pdf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_SessionMonitoring_SessionMonitoringID",
                        column: x => x.SessionMonitoringID,
                        principalTable: "SessionMonitoring",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Slide = table.Column<int>(type: "int", nullable: false),
                    QuestionName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Score = table.Column<float>(type: "float", nullable: false),
                    PDFId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Pdf_PDFId",
                        column: x => x.PDFId,
                        principalTable: "Pdf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_Session_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => new { x.UserId, x.SessionId });
                    table.ForeignKey(
                        name: "FK_UserSession_Session_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSession_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnswerResponse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnswerScore = table.Column<float>(type: "float", nullable: false),
                    AnswerCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QuestionID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionID",
                table: "Answer",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Module_CourseId",
                table: "Module",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Pdf_ModuleID",
                table: "Pdf",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_PDFId",
                table: "Question",
                column: "PDFId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_SessionID",
                table: "Question",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Session_ModuleID",
                table: "Session",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_Session_PdfDocumentID",
                table: "Session",
                column: "PdfDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Session_SessionMonitoringID",
                table: "Session",
                column: "SessionMonitoringID");

            migrationBuilder.CreateIndex(
                name: "IX_User_CourseId",
                table: "User",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionId",
                table: "UserSession",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Pdf");

            migrationBuilder.DropTable(
                name: "SessionMonitoring");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
