﻿using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "EconomicBuilding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoldIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodIncome = table.Column<int>(type: "INTEGER", nullable: false),
                    TickReduction = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBuilt = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomicBuilding", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enemy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ArmyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enemy_Army_ArmyId",
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
                    GoldAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodAmount = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "MilitaryBuilding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatableUnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBuilt = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldCost = table.Column<int>(type: "INTEGER", nullable: false),
                    WoodCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilitaryBuilding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilitaryBuilding_Unit_CreatableUnitId",
                        column: x => x.CreatableUnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_UnitGroup_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enemy_ArmyId",
                table: "Enemy",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryBuilding_CreatableUnitId",
                table: "MilitaryBuilding",
                column: "CreatableUnitId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EconomicBuilding");

            migrationBuilder.DropTable(
                name: "Enemy");

            migrationBuilder.DropTable(
                name: "MilitaryBuilding");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "UnitGroup");

            migrationBuilder.DropTable(
                name: "Army");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}