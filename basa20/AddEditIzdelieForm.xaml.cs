using basa20.Models;
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
using System.Windows.Shapes;

namespace basa20
{
    /// <summary>
    /// Логика взаимодействия для AddEditIzdelieForm.xaml
    /// </summary>
    public partial class AddEditIzdelieForm : Window
    {
        public object Record { get; private set; }


        public AddEditIzdelieForm(object record)
        {
            InitializeComponent();
            Record = record;

            if (record is Models.Изделия изделие)
            {
                Field1TextBox.Text = изделие.НаименованиеИзделия;
                Field2TextBox.Text = изделие.СтоимостьСборки.ToString();
            }
            else if (record is Models.Детали деталь)
            {
                Field1TextBox.Text = деталь.НаименованиеДетали;
                Field2TextBox.Text = деталь.Цена.ToString();
            }
            else if (record is Models.Цеха цех)
            {
                Field1TextBox.Text = цех.НаименованиеЦеха;
                Field2TextBox.Text = цех.Начальник;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Record is Models.Изделия изделие)
            {
                изделие.НаименованиеИзделия = Field1TextBox.Text;
                изделие.СтоимостьСборки = decimal.Parse(Field2TextBox.Text);
            }
            else if (Record is Models.Детали деталь)
            {
                деталь.НаименованиеДетали = Field1TextBox.Text;
                деталь.Цена = decimal.Parse(Field2TextBox.Text);
            }
            else if (Record is Models.Цеха цех)
            {
                цех.НаименованиеЦеха = Field1TextBox.Text;
                цех.Начальник = Field2TextBox.Text;
            }

            DialogResult = true;
            Close();
        }
    }
}