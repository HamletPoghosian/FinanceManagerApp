using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagerApp.FinancePro
{
    class Program
    {
        static void Main(string[] args)

        {
            FinanceDataBase finDB = new FinanceDataBase("Finance");
            finDB.CreateDB().Wait();
            finDB.CreateTable().Wait();
            finDB.InsertDB();
        }
    }
}
