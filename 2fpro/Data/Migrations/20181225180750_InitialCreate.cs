using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _2fpro.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "GetSeq");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Delivery",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payment",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sirname",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(maxLength: 300, nullable: true),
                    Sequance = table.Column<int>(nullable: true),
                    CatDescription = table.Column<string>(nullable: true),
                    CatType = table.Column<string>(maxLength: 300, nullable: true),
                    Sortindex = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR GetSeq")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfigID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(maxLength: 150, nullable: true),
                    Robots = table.Column<string>(maxLength: 500, nullable: true),
                    SiteAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(maxLength: 150, nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    SelectedIsOnlineID = table.Column<bool>(nullable: false),
                    OfflineMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigID);
                });

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GalleryTitle = table.Column<string>(maxLength: 500, nullable: true),
                    GalleryData = table.Column<byte[]>(nullable: true),
                    Sortindex = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR GetSeq"),
                    GalleryMimeType = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ImportProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<string>(nullable: true),
                    ImgUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiveUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    FeedMessage = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    ConnId = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveUsers", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Menues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: false),
                    Body = table.Column<string>(nullable: true),
                    BodyEng = table.Column<string>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: false),
                    CustomField = table.Column<string>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    SeoKeywords = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR GetSeq"),
                    MenuSection = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 300, nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    OrderStatus = table.Column<string>(maxLength: 300, nullable: true),
                    Country = table.Column<string>(maxLength: 300, nullable: true),
                    Delivery = table.Column<string>(maxLength: 300, nullable: true),
                    Payment = table.Column<string>(maxLength: 300, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    OrderSum = table.Column<float>(nullable: false),
                    Sequance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Sequance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageData = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Price = table.Column<int>(nullable: true),
                    Sortindex = table.Column<int>(nullable: true, defaultValueSql: "NEXT VALUE FOR GetSeq"),
                    ProdLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    Body = table.Column<string>(nullable: false),
                    PreviewPhoto = table.Column<byte[]>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Preview = table.Column<string>(maxLength: 500, nullable: true),
                    Sortindex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StaticSections",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 300, nullable: true),
                    Sequance = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Preview = table.Column<string>(maxLength: 500, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    SectionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticSections", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CatName = table.Column<string>(maxLength: 500, nullable: true),
                    Size = table.Column<string>(maxLength: 500, nullable: true),
                    Packaging = table.Column<string>(maxLength: 500, nullable: true),
                    PackagingSize = table.Column<float>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Manufacturer = table.Column<string>(maxLength: 500, nullable: true),
                    Cloth = table.Column<string>(maxLength: 500, nullable: true),
                    Color = table.Column<string>(maxLength: 500, nullable: true),
                    Lacquering = table.Column<string>(maxLength: 500, nullable: true),
                    Decor = table.Column<string>(maxLength: 500, nullable: true),
                    Discount = table.Column<int>(nullable: false),
                    Material = table.Column<string>(maxLength: 500, nullable: true),
                    Fill = table.Column<string>(maxLength: 500, nullable: true),
                    VisitesCount = table.Column<int>(nullable: false),
                    ProductType = table.Column<string>(nullable: true),
                    Sortindex = table.Column<int>(nullable: true, defaultValueSql: "NEXT VALUE FOR GetSeq"),
                    CategoryID = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageTitle = table.Column<string>(maxLength: 500, nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    GalleryID = table.Column<int>(nullable: false),
                    ImageMimeType = table.Column<string>(maxLength: 100, nullable: true),
                    Sortindex = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR GetSeq")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Galleries_GalleryID",
                        column: x => x.GalleryID,
                        principalTable: "Galleries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiveMessages",
                columns: table => new
                {
                    MessID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TextMess = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Visited = table.Column<bool>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveMessages", x => x.MessID);
                    table.ForeignKey(
                        name: "FK_LiveMessages_LiveUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "LiveUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(maxLength: 300, nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Category = table.Column<string>(maxLength: 300, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Cloth = table.Column<string>(maxLength: 500, nullable: true),
                    Color = table.Column<string>(maxLength: 500, nullable: true),
                    Screed = table.Column<string>(maxLength: 500, nullable: true),
                    Molding = table.Column<string>(maxLength: 500, nullable: true),
                    Discount = table.Column<int>(nullable: false),
                    LastPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageDataType = table.Column<byte[]>(nullable: true),
                    ImageMimeType = table.Column<string>(maxLength: 300, nullable: true),
                    IsPreview = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Sortindex = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR GetSeq")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProdImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Configs",
                columns: new[] { "ConfigID", "Description", "Email", "Keywords", "OfflineMessage", "Robots", "SelectedIsOnlineID", "SiteAddress", "SiteName" },
                values: new object[] { 1, null, null, null, null, null, false, null, "sitename" });

            migrationBuilder.InsertData(
                table: "Menues",
                columns: new[] { "Id", "Body", "BodyEng", "CustomField", "IsPublish", "LastModifiedDate", "MenuSection", "ParentId", "SeoDescription", "SeoKeywords", "SortOrder", "Text", "Url" },
                values: new object[] { 1, null, null, null, false, new DateTime(2018, 12, 25, 21, 7, 50, 94, DateTimeKind.Local), 0, 0, null, null, 0, "title", "Home" });

            migrationBuilder.InsertData(
                table: "StaticSections",
                columns: new[] { "ID", "Content", "CreatedAt", "Preview", "SectionType", "Sequance", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "ss", null, null, 2, 1, "static2", 0 },
                    { 2, "ss", null, null, 3, 2, "static3", 0 },
                    { 3, "ss", null, null, 4, 3, "static4", 0 },
                    { 4, "ss", null, null, 5, 4, "static5", 0 },
                    { 5, "ss", null, null, 6, 5, "static6", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_GalleryID",
                table: "Images",
                column: "GalleryID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveMessages_UserID",
                table: "LiveMessages",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProdImages_ProductID",
                table: "ProdImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ImportProducts");

            migrationBuilder.DropTable(
                name: "LiveMessages");

            migrationBuilder.DropTable(
                name: "Menues");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "ProdImages");

            migrationBuilder.DropTable(
                name: "StaticSections");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropTable(
                name: "LiveUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropSequence(
                name: "GetSeq");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sirname",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
