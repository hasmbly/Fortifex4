using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fortifex4.Infrastructure.Persistence.Migrations
{
    public partial class Fortifex4DB_001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blockchains",
                columns: table => new
                {
                    BlockchainID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    Symbol = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blockchains", x => x.BlockchainID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "varchar(5)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderID);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    ProviderID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    ProviderType = table.Column<int>(nullable: false),
                    SiteURL = table.Column<string>(type: "varchar(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.ProviderID);
                });

            migrationBuilder.CreateTable(
                name: "TimeFrames",
                columns: table => new
                {
                    TimeFrameID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeFrames", x => x.TimeFrameID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    BlockchainID = table.Column<int>(nullable: false),
                    CoinMarketCapID = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    CurrencyType = table.Column<int>(nullable: false),
                    IsShownInTradePair = table.Column<bool>(nullable: false),
                    IsForPreferredOption = table.Column<bool>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    UnitPriceInUSD = table.Column<decimal>(type: "decimal(29,20)", nullable: false),
                    Volume24h = table.Column<decimal>(type: "decimal(29,10)", nullable: false),
                    PercentChange1h = table.Column<float>(type: "real", nullable: false),
                    PercentChange24h = table.Column<float>(type: "real", nullable: false),
                    PercentChange7d = table.Column<float>(type: "real", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
                    table.ForeignKey(
                        name: "FK_Currencies_Blockchains_BlockchainID",
                        column: x => x.BlockchainID,
                        principalTable: "Blockchains",
                        principalColumn: "BlockchainID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "varchar(5)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionID);
                    table.ForeignKey(
                        name: "FK_Regions_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberUsername = table.Column<string>(type: "varchar(100)", nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    ExternalID = table.Column<string>(type: "varchar(50)", nullable: true),
                    AuthenticationScheme = table.Column<string>(type: "varchar(10)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    PictureURL = table.Column<string>(type: "varchar(2000)", nullable: true),
                    GenderID = table.Column<int>(nullable: false),
                    RegionID = table.Column<int>(nullable: false),
                    PreferredFiatCurrencyID = table.Column<int>(nullable: false),
                    PreferredCoinCurrencyID = table.Column<int>(nullable: false),
                    PreferredTimeFrameID = table.Column<int>(nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    PasswordSalt = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    ActivationCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivationStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberUsername);
                    table.ForeignKey(
                        name: "FK_Members_Genders_GenderID",
                        column: x => x.GenderID,
                        principalTable: "Genders",
                        principalColumn: "GenderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Currencies_PreferredCoinCurrencyID",
                        column: x => x.PreferredCoinCurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_Currencies_PreferredFiatCurrencyID",
                        column: x => x.PreferredFiatCurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_TimeFrames_PreferredTimeFrameID",
                        column: x => x.PreferredTimeFrameID,
                        principalTable: "TimeFrames",
                        principalColumn: "TimeFrameID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    OwnerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    MemberUsername = table.Column<string>(type: "varchar(100)", nullable: false),
                    ProviderID = table.Column<int>(nullable: false),
                    ProviderType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerID);
                    table.ForeignKey(
                        name: "FK_Owners_Members_MemberUsername",
                        column: x => x.MemberUsername,
                        principalTable: "Members",
                        principalColumn: "MemberUsername",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Owners_Providers_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Providers",
                        principalColumn: "ProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    MemberUsername = table.Column<string>(type: "varchar(100)", nullable: false),
                    BlockchainID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    WalletAddress = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Blockchains_BlockchainID",
                        column: x => x.BlockchainID,
                        principalTable: "Blockchains",
                        principalColumn: "BlockchainID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Members_MemberUsername",
                        column: x => x.MemberUsername,
                        principalTable: "Members",
                        principalColumn: "MemberUsername",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    OwnerID = table.Column<int>(nullable: false),
                    BlockchainID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    ProviderType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletID);
                    table.ForeignKey(
                        name: "FK_Wallets_Blockchains_BlockchainID",
                        column: x => x.BlockchainID,
                        principalTable: "Blockchains",
                        principalColumn: "BlockchainID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Wallets_Owners_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Owners",
                        principalColumn: "OwnerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    ContributorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false),
                    MemberUsername = table.Column<string>(type: "varchar(100)", nullable: false),
                    InvitationCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.ContributorID);
                    table.ForeignKey(
                        name: "FK_Contributors_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pockets",
                columns: table => new
                {
                    PocketID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    WalletID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    CurrencyType = table.Column<int>(nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", nullable: false),
                    IsMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pockets", x => x.PocketID);
                    table.ForeignKey(
                        name: "FK_Pockets_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pockets_Wallets_WalletID",
                        column: x => x.WalletID,
                        principalTable: "Wallets",
                        principalColumn: "WalletID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false),
                    PocketID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(29,10)", nullable: false),
                    UnitPriceInUSD = table.Column<decimal>(type: "decimal(29,20)", nullable: false),
                    TransactionHash = table.Column<string>(type: "varchar(100)", nullable: true),
                    PairWalletName = table.Column<string>(type: "varchar(200)", nullable: true),
                    PairWalletAddress = table.Column<string>(type: "varchar(200)", nullable: true),
                    TransactionType = table.Column<int>(nullable: false),
                    TransactionDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Pockets_PocketID",
                        column: x => x.PocketID,
                        principalTable: "Pockets",
                        principalColumn: "PocketID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternalTransfers",
                columns: table => new
                {
                    InternalTransferID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromTransactionID = table.Column<int>(nullable: false),
                    ToTransactionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalTransfers", x => x.InternalTransferID);
                    table.ForeignKey(
                        name: "FK_InternalTransfers_Transactions_FromTransactionID",
                        column: x => x.FromTransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalTransfers_Transactions_ToTransactionID",
                        column: x => x.ToTransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromTransactionID = table.Column<int>(nullable: false),
                    ToTransactionID = table.Column<int>(nullable: false),
                    TradeType = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(29,20)", nullable: false),
                    IsWithholding = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradeID);
                    table.ForeignKey(
                        name: "FK_Trades_Transactions_FromTransactionID",
                        column: x => x.FromTransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trades_Transactions_ToTransactionID",
                        column: x => x.ToTransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contributors_ProjectID",
                table: "Contributors",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_BlockchainID",
                table: "Currencies",
                column: "BlockchainID");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfers_FromTransactionID",
                table: "InternalTransfers",
                column: "FromTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfers_ToTransactionID",
                table: "InternalTransfers",
                column: "ToTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GenderID",
                table: "Members",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PreferredCoinCurrencyID",
                table: "Members",
                column: "PreferredCoinCurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PreferredFiatCurrencyID",
                table: "Members",
                column: "PreferredFiatCurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PreferredTimeFrameID",
                table: "Members",
                column: "PreferredTimeFrameID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RegionID",
                table: "Members",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_MemberUsername",
                table: "Owners",
                column: "MemberUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_ProviderID",
                table: "Owners",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Pockets_CurrencyID",
                table: "Pockets",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Pockets_WalletID",
                table: "Pockets",
                column: "WalletID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BlockchainID",
                table: "Projects",
                column: "BlockchainID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MemberUsername",
                table: "Projects",
                column: "MemberUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryCode",
                table: "Regions",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_FromTransactionID",
                table: "Trades",
                column: "FromTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_ToTransactionID",
                table: "Trades",
                column: "ToTransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PocketID",
                table: "Transactions",
                column: "PocketID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_BlockchainID",
                table: "Wallets",
                column: "BlockchainID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerID",
                table: "Wallets",
                column: "OwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "InternalTransfers");

            migrationBuilder.DropTable(
                name: "Trades");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Pockets");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "TimeFrames");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Blockchains");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
