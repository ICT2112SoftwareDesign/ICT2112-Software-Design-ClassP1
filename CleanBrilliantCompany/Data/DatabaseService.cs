using Microsoft.Data.SqlClient;
using System.Data;

namespace CleanBrilliantCompany.Data
{
    public class DatabaseService : IDisposable
    {
        private readonly SqlConnection _connection;

        public DatabaseService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var command = _connection.CreateCommand();
            // Drop tables if they exist (for testing purposes)
            command.CommandText = @"
                IF OBJECT_ID('Alerts') IS NOT NULL DROP TABLE Alerts;
                IF OBJECT_ID('InventoryLevel') IS NOT NULL DROP TABLE InventoryLevel;
                IF OBJECT_ID('StockStatus') IS NOT NULL DROP TABLE StockStatus;
                IF OBJECT_ID('AlertType') IS NOT NULL DROP TABLE AlertType;
                IF OBJECT_ID('InventoryDashboards') IS NOT NULL DROP TABLE InventoryDashboards;";
            command.ExecuteNonQuery();

            // Create InventoryDashboards table
            command.CommandText = @"
                CREATE TABLE InventoryDashboards (
                    DashboardId INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NOT NULL,
                    RequestedStartDate DATE NOT NULL,
                    RequestedEndDate DATE NOT NULL,
                    GeneratedDate DATETIME,
                    ValidityDuration INT NOT NULL,
                    LevelOfDetail INT NOT NULL
                )";
            command.ExecuteNonQuery();

            // Create StockStatus table
            command.CommandText = @"
                CREATE TABLE StockStatus (
                    StockCode CHAR(1) PRIMARY KEY,
                    Description NVARCHAR(50) NOT NULL
                )";
            command.ExecuteNonQuery();

            // Create AlertType table
            command.CommandText = @"
                CREATE TABLE AlertType (
                    TypeCode CHAR(1) PRIMARY KEY,
                    Description NVARCHAR(50) NOT NULL
                )";
            command.ExecuteNonQuery();

            // Create InventoryLevel table
            command.CommandText = @"
                CREATE TABLE InventoryLevel (
                    InventoryId INT PRIMARY KEY IDENTITY(1,1),
                    ProductId INT NOT NULL,
                    StockLevel INT NOT NULL,
                    Threshold INT NOT NULL,
                    StockCode CHAR(1) NOT NULL,
                    ReplenishmentStatus BIT NOT NULL,
                    DashboardId INT NOT NULL,
                    FOREIGN KEY (StockCode) REFERENCES StockStatus(StockCode),
                    FOREIGN KEY (DashboardId) REFERENCES InventoryDashboards(DashboardId)
                )";
            command.ExecuteNonQuery();

            // Create Alerts table
            command.CommandText = @"
                CREATE TABLE Alerts (
                    AlertId INT PRIMARY KEY IDENTITY(1,1),
                    InventoryId INT NOT NULL,
                    AlertType CHAR(1) NOT NULL,
                    AlertDate DATETIME NOT NULL,
                    FOREIGN KEY (InventoryId) REFERENCES InventoryLevel(InventoryId),
                    FOREIGN KEY (AlertType) REFERENCES AlertType(TypeCode)
                )";
            command.ExecuteNonQuery();
        }

        public IDbConnection GetConnection() => _connection;

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}