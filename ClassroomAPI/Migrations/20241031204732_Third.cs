using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomAPI.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms",
                column: "BuildingId",
                principalTable: "ShortBuildingInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_ShortBuildingInfo_BuildingId",
                table: "Classrooms",
                column: "BuildingId",
                principalTable: "ShortBuildingInfo",
                principalColumn: "Id");
        }
    }
}
