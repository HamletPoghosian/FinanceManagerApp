using FinanceManagerApp.FinancePro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceMeneger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InitializationFinans initial;
        FinanceDataBase dataBase;
        public MainWindow()
        {
            InitializeComponent();
            dataBase = new FinanceDataBase("Finance");
            initial = new InitializationFinans();
            dataBase.CreateDB().Wait();
            if (comboType.Items.Count<1)
            {
                dataBase.CreateTable().Wait();
                comboType.ItemsSource = initial.SelectCategory(dataBase.DBName, dataBase.ConnectionString);
            }

        }

        private void TextBox_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
