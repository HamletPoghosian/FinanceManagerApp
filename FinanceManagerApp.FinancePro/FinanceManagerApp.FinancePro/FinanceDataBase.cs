using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerApp.FinancePro
{
    public class FinanceDataBase:IDisposable
    {
        private readonly string _connectionString= @"Data Source = (localdb)\MSSQLLocalDB;Integrated Security = True; Pooling=False";
        private SqlConnection connec;

        public FinanceDataBase()
        {
            
        }
        public async Task CreateDB(string DBName)
        {
            string createQuery= @"USE [master]
             IF NOT EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name = '"+ DBName + "')" +
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
            catch (SqlException  )
            {
                Console.WriteLine("Chech You DB");                
            }
            catch (Exception)
            {
                throw;
            }
            

        }







        public void Dispose()
        {
            connec.Dispose();
        }
    }
}
