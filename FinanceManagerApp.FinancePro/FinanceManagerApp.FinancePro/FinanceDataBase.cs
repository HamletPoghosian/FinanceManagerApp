using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerApp.FinancePro
{
    public class FinanceDataBase : IDisposable
    {
        private readonly string _connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Integrated Security = True; Pooling=False";
        private SqlConnection connec;
        InitializationFinans initialFin;
        public string DBName { get; set; }
        public FinanceDataBase(string DatabaseName)
        {
            DBName = DatabaseName;
        }
        public async Task CreateDB()
        {
            string createQuery = @"USE [master]
             IF NOT EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name = '" + DBName + "')" +
             "CREATE DATABASE[" + DBName + "];";
            try
            {
                using (connec = new SqlConnection(_connectionString))
                {
                    connec.Open();
                    using (SqlCommand comm = new SqlCommand(createQuery, connec))
                    {
                        comm.ExecuteNonQueryAsync().Wait();
                        connec.Close();
                    };
                };
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Chech You DB");
            }
            catch (SqlException)
            {
                Console.WriteLine("Chech You DB");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CreateTable()
        {
            StringBuilder stringTable = new StringBuilder();
            stringTable.AppendLine(@" USE Finance
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Category' )
                    CREATE TABLE     [Category] (
                    [Id]    UNIQUEIDENTIFIER NOT NULL default newid(),
                    [Title] NVARCHAR (MAX)   NOT NULL,
                    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
                   );
");
                            stringTable.AppendLine(@"USE Finance
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Wallet' )
                    CREATE TABLE        [Wallet] (
                    [Id]          UNIQUEIDENTIFIER NOT NULL default newid(),
                    [CategoryId]  UNIQUEIDENTIFIER NOT NULL ,
                    [Amount]      MONEY            NOT NULL,
                    [Comment]     NVARCHAR (MAX)   NULL,
                    [Day]         DATETIME2 (7)    NOT NULL,
                    [DateCreated] DATETIME2 (7)    CONSTRAINT [DF_Wallet_DateCreated] DEFAULT (getutcdate()) NOT NULL,
                    CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED ([Id] ASC),
                    CONSTRAINT [FK_Wallet_Category] FOREIGN KEY ([CategoryId]) REFERENCES Category(Id) ON DELETE CASCADE ON UPDATE CASCADE);");
            try
            {
                using (connec = new SqlConnection(_connectionString))
                {
                    connec.Open();
                    using (SqlCommand comm = new SqlCommand(stringTable.ToString(), connec))
                    {
                        comm.ExecuteNonQueryAsync().Wait();
                        connec.Close();
                    };
                };
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Chech You DB");
            }
            catch (SqlException)
            {
                Console.WriteLine("Chech You DB");
            }
            catch (DbException)
            {
                Console.WriteLine("Chech you Db");
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public void InsertDB()
        {
            initialFin = new InitializationFinans();
            initialFin.InsertCategories(DBName, _connectionString);
            initialFin.InsertWallet(DBName, _connectionString,10);
        }


        public void Dispose()
        {
            connec.Dispose();
        }
    }
}
