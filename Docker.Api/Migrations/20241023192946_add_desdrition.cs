using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Docker.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_desdrition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Productcs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Productcs");
        }
    }
}
