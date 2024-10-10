﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditDbForBookInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookInstance_Books_BookId",
                table: "BookInstance");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInstance_Users_UserId",
                table: "BookInstance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookInstance",
                table: "BookInstance");

            migrationBuilder.RenameTable(
                name: "BookInstance",
                newName: "BookInstances");

            migrationBuilder.RenameIndex(
                name: "IX_BookInstance_UserId",
                table: "BookInstances",
                newName: "IX_BookInstances_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookInstance_BookId",
                table: "BookInstances",
                newName: "IX_BookInstances_BookId");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "Books",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookInstances",
                table: "BookInstances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstances_Users_UserId",
                table: "BookInstances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookInstances_Books_BookId",
                table: "BookInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInstances_Users_UserId",
                table: "BookInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookInstances",
                table: "BookInstances");

            migrationBuilder.RenameTable(
                name: "BookInstances",
                newName: "BookInstance");

            migrationBuilder.RenameIndex(
                name: "IX_BookInstances_UserId",
                table: "BookInstance",
                newName: "IX_BookInstance_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookInstances_BookId",
                table: "BookInstance",
                newName: "IX_BookInstance_BookId");

            migrationBuilder.AlterColumn<long>(
                name: "Count",
                table: "Books",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookInstance",
                table: "BookInstance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstance_Books_BookId",
                table: "BookInstance",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstance_Users_UserId",
                table: "BookInstance",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
