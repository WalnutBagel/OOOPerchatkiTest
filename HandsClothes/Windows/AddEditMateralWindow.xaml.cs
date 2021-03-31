using HandsClothes.EFData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static HandsClothes.EFData.DataFrame;

namespace HandsClothes.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditMateralWindow.xaml
    /// </summary>
    public partial class AddEditMateralWindow : Window
    {
        string photoPath = null;

        ObservableCollection<Supplier> supplierList = new ObservableCollection<Supplier>();
        public AddEditMateralWindow()
        {
            InitializeComponent();

            cmbTypeMAterial.ItemsSource = Context.MaterialType.ToList();
            cmbTypeMAterial.DisplayMemberPath = "Name";
            cmbTypeMAterial.SelectedIndex = 0;

            cmbUnitMaterial.ItemsSource = Context.Unit.ToList();
            cmbUnitMaterial.DisplayMemberPath = "Unit";
            cmbUnitMaterial.SelectedIndex = 0;


            cmbSupplier.ItemsSource = Context.Supplier.ToList();
            cmbSupplier.DisplayMemberPath = "Name";
        }
        public AddEditMateralWindow(Material material)
        {
            InitializeComponent();

            cmbTypeMAterial.ItemsSource = Context.MaterialType.ToList();
            cmbTypeMAterial.DisplayMemberPath = "Name";

            cmbUnitMaterial.ItemsSource = Context.Unit.ToList();
            cmbUnitMaterial.DisplayMemberPath = "Unit";


            cmbSupplier.ItemsSource = Context.Supplier.ToList();
            cmbSupplier.DisplayMemberPath = "Name";

            txtName.Text = material.MaterialName;
            txtCount.Text = material.QuanityInStock.ToString();
            txtCountInBox.Text = material.QuanityInPack.ToString();
            txtMinCount.Text = material.MinQuanity.ToString();
            txtPrice.Text = material.Price.ToString();
            cmbTypeMAterial.SelectedIndex = material.MaterialTypeId - 1;
            cmbUnitMaterial.SelectedIndex = material.UnitId - 1;
            //imgMaterial.Source = new BitmapImage(new Uri(material.Image));

            var supMaterial = Context.MaterialSupplier.Where(i => i.MaterialId == material.id).ToList();
        }
        private void btnChooseImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                photoPath = openFileDialog.FileName;
                imgMaterial.Source = new BitmapImage(new Uri(photoPath));
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            var resultClick = MessageBox.Show("Добавить?", "Добавление нового материала", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultClick == MessageBoxResult.Yes)
            {
                Material addMaterial = new Material();
                if (photoPath != null)
                {
                    var format = photoPath.Split('.')[photoPath.Split('.').Length - 1];

                    string namePhoto = $@"\materials\{random.Next()}.{format}";

                    File.Copy(photoPath, $@"..\..\{namePhoto}");
                    addMaterial.PhotoPath = namePhoto;
                }
                addMaterial.MaterialName = txtName.Text;
                addMaterial.MaterialTypeId = cmbTypeMAterial.SelectedIndex + 1;
                addMaterial.Price = Convert.ToDecimal(txtPrice.Text);
                addMaterial.QuanityInStock = Convert.ToInt32(txtCount.Text);
                addMaterial.MinQuanity = Convert.ToInt32(txtMinCount.Text);
                addMaterial.QuanityInPack = Convert.ToInt32(txtCountInBox.Text);
                addMaterial.UnitId = cmbUnitMaterial.SelectedIndex + 1;

                Context.Material.Add(addMaterial); // добавление материала

                // добавление поставщиков для материала


                Context.SaveChanges();

                foreach (var item in supplierList)
                {
                    Context.MaterialSupplier.Add(new MaterialSupplier
                    {
                        MaterialId = addMaterial.id,
                        SupplierId = item.id
                    });
                }

                Context.SaveChanges();

                Close();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddSup_Click(object sender, RoutedEventArgs e)
        {
            if ((supplierList.Where(i => i.id == (cmbSupplier.SelectedValue as Supplier).id).ToList().Count) == 0)
            {
                supplierList.Add(cmbSupplier.SelectedValue as Supplier);
            }

            lvListSupplier.ItemsSource = supplierList;
        }
    }
}
