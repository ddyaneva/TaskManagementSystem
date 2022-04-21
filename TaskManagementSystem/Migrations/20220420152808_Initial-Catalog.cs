using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagementSystem.Migrations
{
    public partial class InitialCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    last_name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    email = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    board_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.board_id);
                    table.ForeignKey(
                        name: "FK_Boards_Accounts_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    project_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    key = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    title = table.Column<string>(type: "VARCHAR(250)", nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.project_id);
                    table.ForeignKey(
                        name: "FK_Projects_Accounts_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardMembers",
                columns: table => new
                {
                    board_id = table.Column<int>(nullable: false),
                    account_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMembers", x => x.board_id);
                    table.ForeignKey(
                        name: "FK_BoardMembers_Accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMembers_Boards_board_id",
                        column: x => x.board_id,
                        principalTable: "Boards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    project_id = table.Column<int>(nullable: false),
                    board_id = table.Column<int>(nullable: false),
                    type = table.Column<int>(type: "VARCHAR(50)", nullable: false),
                    status = table.Column<int>(type: "VARCHAR(50)", nullable: false),
                    description = table.Column<string>(nullable: true),
                    reported_by = table.Column<int>(nullable: false),
                    assigedToaccount_id = table.Column<int>(nullable: true),
                    assigned_to = table.Column<int>(nullable: true),
                    priority = table.Column<int>(type: "VARCHAR(50)", nullable: false),
                    story_points = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_Tasks_Accounts_assigedToaccount_id",
                        column: x => x.assigedToaccount_id,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_board_id",
                        column: x => x.board_id,
                        principalTable: "Boards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_project_id",
                        column: x => x.project_id,
                        principalTable: "Projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Accounts_reported_by",
                        column: x => x.reported_by,
                        principalTable: "Accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_email",
                table: "Accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_account_id",
                table: "BoardMembers",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_name",
                table: "Boards",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boards_owner_id",
                table: "Boards",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_key",
                table: "Projects",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_owner_id",
                table: "Projects",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_assigedToaccount_id",
                table: "Tasks",
                column: "assigedToaccount_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_board_id",
                table: "Tasks",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_project_id",
                table: "Tasks",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_reported_by",
                table: "Tasks",
                column: "reported_by");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardMembers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
