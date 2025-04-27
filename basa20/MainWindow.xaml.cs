using basa20.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace basa20
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProizvodstvoContext db;
        public MainWindow()
        {

            InitializeComponent();
            db = new ProizvodstvoContext();
           
        }
        // Открытие окна "Справочник"
        private void OpenReference_Click(object sender, RoutedEventArgs e)
        {
            var referenceWindow = new IzdeliyaForm();
            referenceWindow.Show();
        }

        // Открытие окна "Поиск"
        private void OpenSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchForm = new SearchForm();
            searchForm.Show();
        }

        // Запрос 1: Подсчет количества различных деталей в изделии
        private void Task1_Click(object sender, RoutedEventArgs e)
        {
            string наименованиеИзделия = IzdelieInput.Text.Trim();

            if (string.IsNullOrEmpty(наименованиеИзделия))
            {
                MessageBox.Show("Введите наименование изделия!");
                return;
            }

            var query = from i in db.Изделияs
                        join si in db.СоставИзделияs on i.КодИзделия equals si.КодИзделия
                        where i.НаименованиеИзделия == наименованиеИзделия
                        group si by i.НаименованиеИзделия into g
                        select new
                        {
                            НаименованиеИзделия = g.Key,
                            Количество_деталей = g.Select(x => x.КодДетали).Distinct().Count()
                        };

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование изделия", Binding = new Binding("НаименованиеИзделия") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Количество деталей", Binding = new Binding("Количество_деталей") });

            MainDataGrid.ItemsSource = query.ToList();

            if (!query.Any())
            {
                MessageBox.Show("Изделие не найдено!");
            }
        }

        // Запрос 2: Подсчет стоимости изделия
        private void Task2_Click(object sender, RoutedEventArgs e)
        {
            string наименованиеИзделия = IzdelieCostInput.Text.Trim();

            if (string.IsNullOrEmpty(наименованиеИзделия))
            {
                MessageBox.Show("Введите наименование изделия!");
                return;
            }

            var query = from i in db.Изделияs
                        join si in db.СоставИзделияs on i.КодИзделия equals si.КодИзделия
                        join d in db.Деталиs on si.КодДетали equals d.КодДетали
                        where i.НаименованиеИзделия == наименованиеИзделия
                        group new { si, d } by new { i.НаименованиеИзделия, i.СтоимостьСборки } into g
                        select new
                        {
                            НаименованиеИзделия = g.Key.НаименованиеИзделия,
                            Стоимость_изделия = g.Sum(x => x.d.Цена * x.si.КоличествоДеталей) + g.Key.СтоимостьСборки
                        };

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование изделия", Binding = new Binding("НаименованиеИзделия") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость изделия", Binding = new Binding("Стоимость_изделия") });

            MainDataGrid.ItemsSource = query.ToList();

            if (!query.Any())
            {
                MessageBox.Show("Изделие не найдено!");
            }
        }
    }
}