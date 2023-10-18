using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goldenwood.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Army",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Army", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ArmyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enemies_Army_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Army",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoldIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    TickInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    ArmyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Army_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Army",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EconomicBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoldIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    TickReduction = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicBuildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EconomicBuildings_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MilitaryBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryBuildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryBuildings_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false),
                    MilitaryBuildingId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_MilitaryBuildings_MilitaryBuildingId",
                        column: x => x.MilitaryBuildingId,
                        principalTable: "MilitaryBuildings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnitGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ArmyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitGroup_Army_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Army",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnitGroup_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EconomicBuildings_PlayerId",
                table: "EconomicBuildings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_ArmyId",
                table: "Enemies",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryBuildings_PlayerId",
                table: "MilitaryBuildings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ArmyId",
                table: "Player",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitGroup_ArmyId",
                table: "UnitGroup",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitGroup_UnitId",
                table: "UnitGroup",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_MilitaryBuildingId",
                table: "Units",
                column: "MilitaryBuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EconomicBuildings");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "UnitGroup");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "MilitaryBuildings");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Army");
        }
    }
}
