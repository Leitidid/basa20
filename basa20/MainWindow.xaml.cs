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
            var referenceWindow = new izdelieforma();
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
        // Запрос 1: Уменьшение стоимости сборки в 1,2 раза
        private void ReduceCost_Click(object sender, RoutedEventArgs e)
        {
            foreach (var изделие in db.Изделияs)
            {
                изделие.СтоимостьСборки /= 1.2m;
            }
            db.SaveChanges();
            MessageBox.Show("Стоимость сборки успешно уменьшена в 1,2 раза.");
        }

        // Запрос 2: Добавление записи в таблицу Цеха
        private void AddCexa_Click(object sender, RoutedEventArgs e)
        {
            var новыйЦех = new Models.Цеха
            {
                НаименованиеЦеха = CexaNameInput.Text,
                Начальник = CexaNachalnikInput.Text
            };
            db.Цехаs.Add(новыйЦех);
            db.SaveChanges();
            MessageBox.Show("Цех успешно добавлен!");
        }

        // Запрос 3: Средняя стоимость изделий по цехам
        private void AverageCostByCexa_Click(object sender, RoutedEventArgs e)
        {
            var query = from цех in db.Цехаs
                        join план in db.ПланВыпускаs on цех.КодЦеха equals план.КодЦеха
                        join изделие in db.Изделияs on план.КодИзделия equals изделие.КодИзделия
                        group изделие by цех.НаименованиеЦеха into g
                        select new
                        {
                            НаименованиеЦеха = g.Key,
                            СредняяСтоимость = g.Average(x => x.СтоимостьСборки)
                        };

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование цеха", Binding = new Binding("НаименованиеЦеха") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Средняя стоимость изделий", Binding = new Binding("СредняяСтоимость") });

            var result = query.ToList();
            MainDataGrid.ItemsSource = result;

            if (!result.Any())
            {
                MessageBox.Show("Данные не найдены!");
            }
        }

        // Запрос 4: План выпуска изделий
        private void PlanVypuska_Click(object sender, RoutedEventArgs e)
        {
            var query = from изделие in db.Изделияs
                        join состав in db.СоставИзделияs on изделие.КодИзделия equals состав.КодИзделия
                        join деталь in db.Деталиs on состав.КодДетали equals деталь.КодДетали
                        group new { состав, деталь } by new
                        {
                            изделие.КодИзделия,
                            изделие.НаименованиеИзделия,
                            изделие.СтоимостьСборки
                        } into g
                        select new
                        {
                            КодИзделия = g.Key.КодИзделия,
                            НаименованиеИзделия = g.Key.НаименованиеИзделия,
                            Количество_деталей = g.Sum(x => x.состав.КоличествоДеталей),
                            СтоимостьСборки = g.Key.СтоимостьСборки,
                            СтоимостьПлана = g.Sum(x => x.состав.КоличествоДеталей * x.деталь.Цена) + g.Key.СтоимостьСборки
                        };

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Код изделия", Binding = new Binding("КодИзделия") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование изделия", Binding = new Binding("НаименованиеИзделия") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Количество деталей", Binding = new Binding("Количество_деталей") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость сборки", Binding = new Binding("СтоимостьСборки") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость планового выпуска", Binding = new Binding("СтоимостьПлана") });

            var result = query.ToList();
            MainDataGrid.ItemsSource = result;

            if (!result.Any())
            {
                MessageBox.Show("Данные не найдены!");
            }
        }

        // Запрос 5: Изделия ниже средней стоимости
        private void BelowAverageCost_Click(object sender, RoutedEventArgs e)
        {
            var средняяСтоимость = db.Изделияs.Average(x => x.СтоимостьСборки);

            var query = from изделие in db.Изделияs
                        where изделие.СтоимостьСборки < средняяСтоимость
                        select new
                        {
                            НаименованиеИзделия = изделие.НаименованиеИзделия,
                            СтоимостьСборки = изделие.СтоимостьСборки
                        };

            MainDataGrid.Columns.Clear();
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование изделия", Binding = new Binding("НаименованиеИзделия") });
            MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость сборки", Binding = new Binding("СтоимостьСборки") });

            var result = query.ToList();
            MainDataGrid.ItemsSource = result;

            if (!result.Any())
            {
                MessageBox.Show("Изделия ниже средней стоимости не найдены!");
            }
        }
    }
}
    
