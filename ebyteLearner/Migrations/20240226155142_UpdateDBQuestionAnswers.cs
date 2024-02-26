using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ebyteLearner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBQuestionAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Session_SessionID",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_SessionID",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AnswerScore",
                table: "Answer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SessionID",
                table: "Question",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<float>(
                name: "AnswerScore",
                table: "Answer",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Question_SessionID",
                table: "Question",
                column: "SessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Session_SessionID",
                table: "Question",
                column: "SessionID",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
