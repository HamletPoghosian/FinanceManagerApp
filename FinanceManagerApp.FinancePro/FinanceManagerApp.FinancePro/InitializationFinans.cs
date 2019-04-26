using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerApp.FinancePro
{
    public class InitializationFinans
    {
        private SqlConnection con;

        public void InsertCategories(string dbName,string sqlconnection)
        {
            Dictionary<string, Guid> categories = new Dictionary<string, Guid>
            {
                { "Salary", Guid.NewGuid() },
                { "Credit", Guid.NewGuid() },
                { "Gasoline", Guid.NewGuid() },
                { "Entertainment", Guid.NewGuid() },
                { "Food", Guid.NewGuid() },
                { "Other", Guid.NewGuid() }

            };
            StringBuilder sqlQuerry = new StringBuilder();
            sqlQuerry.AppendLine($"USE [{dbName}];");
            foreach (var category in categories)
            {
                sqlQuerry.AppendLine($"INSERT INTO [Category]([Id], [Title]) VALUES('{category.Value}', '{category.Key}');");
            }
            using (con=new SqlConnection(sqlconnection))
            {
                con.OpenAsync().Wait();
                using (SqlCommand com=new SqlCommand(sqlQuerry.ToString(),con))
                {
                    com.ExecuteNonQueryAsync().Wait();
                    con.Close();
                };
            };

        }
        public void InsertWallet(string dbName, string sqlconnection)
        {
            StringBuilder sqlQuerry = new StringBuilder();

        }
    }
}
