using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject.Migrations.Entities
{
    public partial class modifygroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contests_contestId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Problems_ProblemId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Users_userId",
                table: "Submission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submission",
                table: "Submission");

            migrationBuilder.RenameTable(
                name: "Submission",
                newName: "Submissions");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_userId",
                table: "Submissions",
                newName: "IX_Submissions_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_ProblemId",
                table: "Submissions",
                newName: "IX_Submissions_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Submission_contestId",
                table: "Submissions",
                newName: "IX_Submissions_contestId");

            migrationBuilder.AddColumn<string>(
                name: "GroupTitle",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsers",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Contests_contestId",
                table: "Submissions",
                column: "contestId",
                principalTable: "Contests",
                principalColumn: "contestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                table: "Submissions",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "ProblemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Users_userId",
                table: "Submissions",
                column: "userId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Contests_contestId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_userId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "GroupTitle",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "NumberOfUsers",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "Submissions",
                newName: "Submission");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_userId",
                table: "Submission",
                newName: "IX_Submission_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_ProblemId",
                table: "Submission",
                newName: "IX_Submission_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_contestId",
                table: "Submission",
                newName: "IX_Submission_contestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submission",
                table: "Submission",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Contests_contestId",
                table: "Submission",
                column: "contestId",
                principalTable: "Contests",
                principalColumn: "contestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Problems_ProblemId",
                table: "Submission",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "ProblemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Users_userId",
                table: "Submission",
                column: "userId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
