using basa20.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для IzdeliyaForm.xaml
    /// </summary>
    public partial class IzdeliyaForm : Window
    {
        private ProizvodstvoContext db;
        public IzdeliyaForm()
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
            CexaGrid.ItemsSource = db.Цехаs.ToList();
            PlanVypuskaGrids.ItemsSource = db.ПланВыпускаs.ToList();
            SostavIzdeliyaGrid.ItemsSource = db.СоставИзделияs.ToList();
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