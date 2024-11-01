using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class Secound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_BuildingId",
                table: "Classrooms",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms",
                column: "BuildingId",
                principalTable: "ShortBuildingInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_BuildingId",
                table: "Classrooms");
        }
    }
}
