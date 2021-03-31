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
using HandsClothes.EFData;
using HandsClothes.Windows;
using static HandsClothes.EFData.DataFrame;

namespace HandsClothes.Pages
{
    public partial class ListOfMaterialsPage : Page
    {
        List<Material> materialList = new List<Material>();

        List<string> FilterList = new List<string>();

        int numberPage = 0;

        public List<Material> selectMaterial;

        List<string> SortList = new List<string>()
        {
            "Наименование (по возрастанию)",
            "Наименование (по убыванию)",
            "Остаток на складе(по возрастанию)",
            "Остаток на складе(по убыванию)",
            "Стоимость(по возрастанию)",
            "Стоимость(по убыванию)"
        };

        public ListOfMaterialsPage()
        {
            InitializeComponent();

            btnEditMinCount.Visibility = Visibility.Collapsed;
            btnEditMaterial.Visibility = Visibility.Collapsed;

            MaterialLV.ItemsSource = Filter();

            var typeMaterial = Context.MaterialType.ToList();
            foreach (var i in typeMaterial)
            {
                FilterList.Add(i.MaterialTypeName);
            }

            FilterList.Insert(0, "Все типы");
            FilterCMB.ItemsSource = FilterList;
            FilterCMB.SelectedIndex = 0;

            SortCMB.ItemsSource = SortList;
            SortCMB.SelectedIndex = 0;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private List<Material> Filter()
        {

            materialList = Context.Material.ToList();

            // Сотрировка

            var SortSelect = SortCMB.SelectedIndex;
            switch (SortSelect)
            {
                case 0:
                    materialList = materialList.OrderBy(i => i.MaterialName).ToList(); // по возрастанию
                    break;

                case 1:
                    materialList = materialList.OrderByDescending(i => i.MaterialName).ToList(); // по убыванию
                    break;

                case 2:
                    materialList = materialList.OrderBy(i => i.QuanityInStock).ToList();
                    break;

                case 3:
                    materialList = materialList.OrderByDescending(i => i.QuanityInStock).ToList();
                    break;

                case 4:
                    materialList = materialList.OrderBy(i => i.Price).ToList();
                    break;

                case 5:
                    materialList = materialList.OrderByDescending(i => i.Price).ToList();
                    break;

                default:
                    materialList = materialList.OrderBy(i => i.id).ToList();
                    break;
            }

            // Поиск

            materialList = materialList.
                Where(i => i.MaterialName.ToLower().Contains(SearchTxt.Text.ToLower())).
                ToList();


            // Фильтрация

            var selectFiltr = FilterCMB.SelectedIndex;

            if (selectFiltr != 0)
            {
                materialList = materialList.Where(i => i.MaterialTypeId == selectFiltr).ToList();
            }

            // Вывод количества записей 

            tbStartCount.Text = "15";
            tbAllCount.Text = materialList.Count.ToString();

            // Вывод по страницам

            materialList = materialList.
                Skip(15 * numberPage).
                Take(15).
                ToList();

            return materialList;
        }

        public void UpdateTable()
        {
            MaterialLV.ItemsSource = Filter();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (numberPage > 0)
            {
                numberPage--;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
            }
            UpdateTable();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (materialList.Count > 0)
            {
                numberPage++;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
            }
            UpdateTable();
        }


        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (materialList.Count > 0)
            {
                numberPage++;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
            }
            UpdateTable();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (materialList.Count > 0)
            {
                numberPage += 2;
                btn1.Content = (numberPage + 2).ToString();
                btn2.Content = (numberPage + 3).ToString();
                btn3.Content = (numberPage + 4).ToString();
            }
            UpdateTable();
        }

        private void SortCMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTable();
        }

        private void FilterCMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTable();
        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTable();
        }

        private void btnEditMinCount_Click(object sender, RoutedEventArgs e)
        {
            selectMaterial = MaterialLV.SelectedItems as List<Material>;

            if (selectMaterial != null)
            {
                HelperClasses.MinQuanityMaterial.getMinQuanity = selectMaterial.Max(i => i.MinQuanity);
            }

            MinQuanityWindow minQuanityWindow = new MinQuanityWindow();
            minQuanityWindow.ShowDialog();

            UpdateTable();
        }

        private void btnEditMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddEditMateralWindow addEditMateralWindow = new AddEditMateralWindow(MaterialLV.SelectedItem as Material);
            this.Opacity = 0.3;
            addEditMateralWindow.ShowDialog();
            UpdateTable();
            this.Opacity = 1;
        }

        private void btnAddMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddEditMateralWindow addEditMateralWindow = new AddEditMateralWindow();
            this.Opacity = 0.3;
            addEditMateralWindow.ShowDialog();
            UpdateTable();
            this.Opacity = 1;
        }

        private void MaterialLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditMinCount.Visibility = Visibility.Visible; // стала видимой кнопка изменения минимального количества
            btnEditMaterial.Visibility = Visibility.Visible; // стала видимой кнопка изменения материала  
        }
    }
}
