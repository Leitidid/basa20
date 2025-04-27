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
    /// Логика взаимодействия для SearchForm.xaml
    /// </summary>
    public partial class SearchForm : Window
    {
        private ProizvodstvoContext db;
        public SearchForm()
        {
            InitializeComponent();
            db = new ProizvodstvoContext();
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Изделияs
                          .Where(i => i.НаименованиеИзделия.Contains(SearchTextBox.Text))
                          .ToList();
            SearchResultsGrid.ItemsSource = query;
        }
    }
}
 