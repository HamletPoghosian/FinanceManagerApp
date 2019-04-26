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
                { "Wear", Guid.NewGuid() },
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
        public List<Guid> SelectCategory(string dbName,string sqlconnection)
        {
            using (con = new SqlConnection(sqlconnection))
            {
                string sqlQuerry = @"USE Finance select Id from Category";
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry, con))
                {
                    SqlDataReader rd = com.ExecuteReader();
                    List<Guid> guid = new List<Guid>();
                   
                    
                    while (rd.Read())
                    {                        
                        guid.Add((Guid)rd["Id"]);                       
                    }                    
                    con.Close();
                    return guid;
                };
            };            
        }
        public void InsertWallet(string dbName, string sqlconnection,int size)
        {
            Random rnd = new Random();
            StringBuilder sqlQuerry = new StringBuilder();
            List<Guid> categories = SelectCategory(dbName, sqlconnection);
            for (int i = 0; i < size; i++)
            {                
            sqlQuerry.AppendLine($"INSERT INTO [Wallet] ([CategoryId], [Amount], [Day]) VALUES('{categories[rnd.Next(categories.Count-1)]}', {rnd.Next(10, 50) * 100}, '{rnd.Next(2010,2019)}-{1,13}-{1,32}');");
            }


        }
    }
}
