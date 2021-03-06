﻿using System;
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

        public string SelectBySolid(string dbName, string sqlconnection,DateTime dataFirst,DateTime dataLast)
        {
            StringBuilder sqlQuerry = new StringBuilder();
            using (con = new SqlConnection(sqlconnection))
            {
                 sqlQuerry.Append( @"use Finance
                                    select sum(Amount)
                                    from  Wallet 
                                    where Wallet.Day between '"+dataFirst+ "'AND '"+ dataLast +"' " );
               
               /* sqlQuerry.Append(@" SELECT      Wallet.Day           
    FROM      Wallet 
    GROUP BY Wallet.Day
    ORDER BY Wallet.Day ");*/
                con.OpenAsync().Wait();
                string BuyValue=string.Empty;
                using (SqlCommand com = new SqlCommand(sqlQuerry.ToString(), con))
                {
                    SqlDataReader rd = com.ExecuteReader();

                    while (rd.Read())
                    {
                        BuyValue=rd[0].ToString();                       
                        break;
                    }
                    con.Close();
                    return BuyValue;
                };
            };
        }

        public string SelectByDay(string dbName, string sqlconnection, DateTime dataFirst, DateTime dataLast)
        {
            StringBuilder sqlQuerry = new StringBuilder();
            using (con = new SqlConnection(sqlconnection))
            {                
                 sqlQuerry.Append(@" use Finance
SELECT      Wallet.Day,
    COUNT(Wallet.Day) AS TopWallet
    FROM      Wallet 
    GROUP BY Wallet.Day
    ORDER BY TopWallet DESC
 ");
                con.OpenAsync().Wait();
                DateTime BuyValue=DateTime.Now;
                using (SqlCommand com = new SqlCommand(sqlQuerry.ToString(), con))
                {
                    SqlDataReader rd = com.ExecuteReader();

                    while (rd.Read())
                    {
                        BuyValue = DateTime.Parse( rd[0].ToString());
                        break;
                    }
                    con.Close();
                    return BuyValue.ToString("MM/dd/yyyy");
                };
            };
        }
        public string SelectByTitle(string dbName, string sqlconnection, DateTime dataFirst, DateTime dataLast)
        {
            StringBuilder sqlQuerry = new StringBuilder();
            using (con = new SqlConnection(sqlconnection))
            {
                sqlQuerry.Append(@" use Finance; 

WITH  w(Title,tit) 
as 
(
select count(Category.Title),Category.Title
 FROM      Wallet
	left join Category
	ON Wallet.CategoryId=Category.Id
   group by Category.Title
   )
select TOP(1) w.tit
   from w
   Order by Title desc
   
 ");
                con.OpenAsync().Wait();
                string BuyValue = string.Empty;
                using (SqlCommand com = new SqlCommand(sqlQuerry.ToString(), con))
                {
                    SqlDataReader rd = com.ExecuteReader();

                    while (rd.Read())
                    {
                        BuyValue =rd[0].ToString();
                        break;
                    }
                    con.Close();
                    return BuyValue;
                };
            };
        }
    }
}

