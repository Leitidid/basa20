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
    /// Логика взаимодействия для izdelieforma.xaml
    /// </summary>
    public partial class izdelieforma : Window
    {
        private ProizvodstvoContext db;
        public izdelieforma()
        {
            InitializeComponent();
            db = new ProizvodstvoContext();
            LoadData();
        }


        private void LoadData()
        {
            // Загрузка данных для каждой вкладки
            IzdeliyaGrid.ItemsSource = db.Изделияs.ToList();
            DetaliGrid.ItemsSource = db.Деталиs.ToList();
           
            PlanVypuskaGrid.ItemsSource = db.ПланВыпускаs.ToList();
            SostavIzdeliyaGrid.ItemsSource = db.СоставИзделияs.ToList();
            CexaGri.ItemsSource = db.Цехаs.ToList();
        }

        // Обработчик кнопки "Добавить"
        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            var form = new AddEditIzdelieForm(new Models.Изделия());
            if (form.ShowDialog() == true)
            {
                db.Изделияs.Add(form.Record as Models.Изделия);
                db.SaveChanges();
                LoadData();
            }
        }

        // Обработчик кнопки "Удалить"
        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            var selected = IzdeliyaGrid.SelectedItem as Models.Изделия;
            if (selected != null)
            {
                db.Изделияs.Remove(selected);
                db.SaveChanges();
                LoadData();
            }
        }
    }
}