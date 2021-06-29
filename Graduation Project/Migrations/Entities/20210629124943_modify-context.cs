using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProject.Migrations.Entities
{
    public partial class modifycontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Blog_blogId1",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Groups_groupId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogTag_Blog_BlogId",
                table: "BlogTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Blog_blogId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_commentVote_Comment_commentId",
                table: "commentVote");

            migrationBuilder.DropForeignKey(
                name: "FK_Handle_Users_UserId",
                table: "Handle");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Contests_contestId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Problems_ProblemId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Users_userId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBlog_Blog_blogId",
                table: "UserBlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submission",
                table: "Submission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Handle",
                table: "Handle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blog",
                table: "Blog");

            migrationBuilder.RenameTable(
                name: "Submission",
                newName: "Submissions");

            migrationBuilder.RenameTable(
                name: "Handle",
                newName: "Handles");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Blog",
                newName: "Blogs");

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

            migrationBuilder.RenameIndex(
                name: "IX_Handle_UserId",
                table: "Handles",
                newName: "IX_Handles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_blogId",
                table: "Comments",
                newName: "IX_Comments_blogId");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_groupId",
                table: "Blogs",
                newName: "IX_Blogs_groupId");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_blogId1",
                table: "Blogs",
                newName: "IX_Blogs_blogId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "SubmissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Handles",
                table: "Handles",
                column: "handleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "commentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs",
                column: "blogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Blogs_blogId1",
                table: "Blogs",
                column: "blogId1",
                principalTable: "Blogs",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Groups_groupId",
                table: "Blogs",
                column: "groupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTag_Blogs_BlogId",
                table: "BlogTag",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_blogId",
                table: "Comments",
                column: "blogId",
                principalTable: "Blogs",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_commentVote_Comments_commentId",
                table: "commentVote",
                column: "commentId",
                principalTable: "Comments",
                principalColumn: "commentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Handles_Users_UserId",
                table: "Handles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserBlog_Blogs_blogId",
                table: "UserBlog",
                column: "blogId",
                principalTable: "Blogs",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Blogs_blogId1",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Groups_groupId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogTag_Blogs_BlogId",
                table: "BlogTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_blogId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_commentVote_Comments_commentId",
                table: "commentVote");

            migrationBuilder.DropForeignKey(
                name: "FK_Handles_Users_UserId",
                table: "Handles");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Contests_contestId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Problems_ProblemId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_userId",
                table: "Submissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBlog_Blogs_blogId",
                table: "UserBlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Handles",
                table: "Handles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Submissions",
                newName: "Submission");

            migrationBuilder.RenameTable(
                name: "Handles",
                newName: "Handle");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "Blogs",
                newName: "Blog");

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

            migrationBuilder.RenameIndex(
                name: "IX_Handles_UserId",
                table: "Handle",
                newName: "IX_Handle_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_blogId",
                table: "Comment",
                newName: "IX_Comment_blogId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_groupId",
                table: "Blog",
                newName: "IX_Blog_groupId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_blogId1",
                table: "Blog",
                newName: "IX_Blog_blogId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submission",
                table: "Submission",
                column: "SubmissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Handle",
                table: "Handle",
                column: "handleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "commentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blog",
                table: "Blog",
                column: "blogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Blog_blogId1",
                table: "Blog",
                column: "blogId1",
                principalTable: "Blog",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Groups_groupId",
                table: "Blog",
                column: "groupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTag_Blog_BlogId",
                table: "BlogTag",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Blog_blogId",
                table: "Comment",
                column: "blogId",
                principalTable: "Blog",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_commentVote_Comment_commentId",
                table: "commentVote",
                column: "commentId",
                principalTable: "Comment",
                principalColumn: "commentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Handle_Users_UserId",
                table: "Handle",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserBlog_Blog_blogId",
                table: "UserBlog",
                column: "blogId",
                principalTable: "Blog",
                principalColumn: "blogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
