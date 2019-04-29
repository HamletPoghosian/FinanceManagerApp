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

        public void InsertCategories(string dbName, string sqlconnection)
        {

            Dictionary<string, Guid> categories = new Dictionary<string, Guid>
            {
                { "Salary", Guid.NewGuid() },
                { "Wear",   Guid.NewGuid() },
                { "Credit", Guid.NewGuid() },
                { "Gasoline", Guid.NewGuid() },
                { "Entertainment", Guid.NewGuid() },
                { "Food",   Guid.NewGuid() },
                { "Other",  Guid.NewGuid() }
            };
            StringBuilder sqlQuerry = new StringBuilder();
            sqlQuerry.AppendLine($"USE [{dbName}];");
            foreach (var category in categories)
            {
                sqlQuerry.AppendLine($"INSERT INTO [Category]([Id], [Title]) VALUES('{category.Value}', '{category.Key}');");
            }
            using (con = new SqlConnection(sqlconnection))
            {
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry.ToString(), con))
                {
                    com.ExecuteNonQueryAsync().Wait();
                    con.Close();
                };
            };
        }
        public List<string> SelectCategory(string dbName, string sqlconnection)
        {
            using (con = new SqlConnection(sqlconnection))
            {
                string sqlQuerry = @"USE Finance select Title from Category";
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry, con))
                {
                    SqlDataReader rd = com.ExecuteReader();
                    List<string> CategoryValue = new List<string>();

                    while (rd.Read())
                    {
                        CategoryValue.Add(rd["Title"].ToString());
                    }
                    con.Close();
                    return CategoryValue;
                };
            };
        }
        public Guid SelectCategory(string dbName, string sqlconnection,string category)
        {
            using (con = new SqlConnection(sqlconnection))
            {
                string sqlQuerry = @"USE Finance select Id from Category Where Title='"+ category + "'";
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry, con))
                {
                    SqlDataReader rd = com.ExecuteReader();
                    Guid value = new Guid();

                    while (rd.Read())
                    {
                        value =(Guid)rd["Id"];
                        com.Clone();
                    }
                   
                    return value;
                };
            };
        }
        public void InsertWallet(string dbName, string sqlconnection, int coin, string category, string comment, DateTime data)
        {            
            StringBuilder sqlQuerry = new StringBuilder();
            Guid value = SelectCategory(dbName, sqlconnection, category);
            sqlQuerry.Append(@"USE Finance ");
            sqlQuerry.AppendLine($"INSERT INTO Wallet ([CategoryId], [Amount],[Comment], [Day],[DateCreated]) VALUES('{value}', {coin}, '{comment}','{data}','{DateTime.Now}');");
            using (con = new SqlConnection(sqlconnection))
            {
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry.ToString(), con))
                {
                    com.ExecuteNonQueryAsync().Wait();
                    con.Close();
                };
            };
        }

        public List<string> SelectBuy(string dbName, string sqlconnection,DateTime dataFirst,DateTime dataLast)
        {
            using (con = new SqlConnection(sqlconnection))
            {
                string sqlQuerry = @"use Finance
                                    select sum(Amount)
                                    from  Wallet 
                                    where Wallet.Day between '"+dataFirst+ "'AND '"+ dataLast +"'";
                con.OpenAsync().Wait();
                using (SqlCommand com = new SqlCommand(sqlQuerry, con))
                {
                    SqlDataReader rd = com.ExecuteReader();
                    List<string> BuyValue = new List<string>();

                    while (rd.Read())
                    {
                        BuyValue.Add(rd["Amount"].ToString());
                    }
                    con.Close();
                    return BuyValue;
                };
            };
        }
    }
}

