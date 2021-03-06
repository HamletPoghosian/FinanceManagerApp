﻿using FinanceManagerApp.FinancePro;
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
            dataBase.CreateTable().Wait();
            comboType.ItemsSource = initial.SelectCategory(dataBase.DBName, dataBase.ConnectionString);
            if (comboType.Items.Count < 1)
            {
                initial.InsertCategories(dataBase.DBName, dataBase.ConnectionString);
                comboType.ItemsSource = initial.SelectCategory(dataBase.DBName, dataBase.ConnectionString);
            }

        }

        private void TextBox_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            initial = new InitializationFinans();
            try
            {
                initial.InsertWallet(dataBase.DBName, dataBase.ConnectionString, int.Parse(textnumber.Text), comboType.SelectedValue.ToString(), textComment.Text, DateTime.Parse(textCalendar.Text));
                MessageBox.Show("Տվյալները հաջողությամբ գրանցվեցին");
            }
            catch (Exception)
            {
                MessageBox.Show("Տվյալները  չեն գրանցվել ");
            }

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            initial = new InitializationFinans();
          txtMoney.Text= initial.SelectBySolid(dataBase.DBName,dataBase.ConnectionString, DateTime.Parse(DataFirst.SelectedDate.ToString()),DateTime.Parse( DataLast.SelectedDate.ToString()));
          TxtDay.Text= initial.SelectByDay(dataBase.DBName, dataBase.ConnectionString, DateTime.Parse(DataFirst.SelectedDate.ToString()), DateTime.Parse(DataLast.SelectedDate.ToString()));
            txtType.Text = initial.SelectByTitle(dataBase.DBName, dataBase.ConnectionString, DateTime.Parse(DataFirst.SelectedDate.ToString()), DateTime.Parse(DataLast.SelectedDate.ToString()));
        }
    }
}
