using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamProject.Migrations
{
    /// <inheritdoc />
    public partial class ModifyExamSumbission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswer",
                table: "ExamSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrongAnswer",
                table: "ExamSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9488687e-db59-4742-807f-736b97e50fae", "AQAAAAIAAYagAAAAELN+3Y2HG1n/xzZ0bRPhlLKi7I3+FalICuBDD4U0Sonkr4O6dWFLrDlabV6wTjUN+A==", "edc92646-da9e-4b6b-b985-f526a4fe8ad4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "ExamSubmissions");

            migrationBuilder.DropColumn(
                name: "WrongAnswer",
                table: "ExamSubmissions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "daa974ce-acc5-48d8-b87c-16257a5c400e", "AQAAAAIAAYagAAAAEJvT1otOEd7h84Xd1RcNZiWBGArBNy8hRVQq9tiTfZpWCGo+zykd1z7CWfWSture2A==", "04f6669d-6133-40a8-bca6-e1c939756053" });
        }
    }
}
