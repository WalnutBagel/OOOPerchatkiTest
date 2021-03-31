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

namespace HandsClothes.Windows
{
    /// <summary>
    /// Логика взаимодействия для MinQuanityWindow.xaml
    /// </summary>
    public partial class MinQuanityWindow : Window
    {
        public MinQuanityWindow()
        {
            InitializeComponent();
            txtMinCount.Text = HelperClasses.MinQuanityMaterial.getMinQuanity.ToString();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var resultMsg = MessageBox.Show("Нажмите Да для подтверждения", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultMsg == MessageBoxResult.Yes)
            {
                HelperClasses.MinQuanityMaterial.getMinQuanity = Int32.Parse(txtMinCount.Text);
                Close();
            }
        }
    }
}
