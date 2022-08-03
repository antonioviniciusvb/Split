using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Markup;
using MahApps.Metro.Controls;
using Split.Model;
using Split.View;

namespace Split
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ViewModel.ViewModelMain viewModel;

        public MainWindow()
        {
            //Setando a culture atual
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            InitializeComponent();

            getInfoAssembly();

            //var teste = "{\"Id\":3,\"Name\":\"CREDIT_SERILAIZADOR\",\"File\":\"COB_IMOB_L1_A.pdf\",\"Height\":297.0,\"Width\":209.9,\"Fields\":[],\"IsSelected\":false}";
            //var hex = Util.IO.String_To_Hex(teste);
            //var ansi = Util.IO.HextoAnsi(hex);

            viewModel = new ViewModel.ViewModelMain();
            this.DataContext = viewModel;
            trvLayouts.ItemsSource = viewModel.Layouts;
            cbxLayouts.ItemsSource = viewModel.Layouts;
        }


        /// <summary>
        /// Método para inserir o número da versão do Assembly no text do form 
        /// </summary>
        private void getInfoAssembly()
        {
            var aux = AssemblyName.GetAssemblyName("Split.exe");
            this.Title += String.Format(" -  Version: {0}", aux.Version);
            return;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (trvLayouts.SelectedItem is Field)
            {
                var item = trvLayouts.SelectedItem as Field;
                var editField = new EditField(viewModel.Layouts, $"{item.Id}")
                {
                    Owner = App.Current.MainWindow
                };

                editField.ShowDialog();
            }
            else
            if(trvLayouts.SelectedItem is Layout)
            {
                var item = trvLayouts.SelectedItem as Layout;

                var editField = new EditField(viewModel.Layouts, $"{item.Id}")
                {
                    Owner = App.Current.MainWindow
                };

                editField.ShowDialog();
            }
        }

        private void AddLayout(object sender, MouseButtonEventArgs e)
        {
            var editLayout = new EditLayout(viewModel.Layouts);
            editLayout.Owner = App.Current.MainWindow;
            editLayout.ShowDialog();
        }

        private void DeleteItem(object sender, MouseButtonEventArgs e)
        {
            deleteItem();
        }

        private void deleteItem()
        {
            string layoutOrField = string.Empty, name = string.Empty;

            if (trvLayouts.SelectedItem is Field)
            {
                var item = trvLayouts.SelectedItem as Field;

                var deletField = new DeleteField(viewModel.Layouts, $"{item.Id}")
                {
                    Owner = App.Current.MainWindow
                };

                deletField.ShowDialog();

            }
            else
            if (trvLayouts.SelectedItem is Layout)
            {
                var layout = trvLayouts.SelectedItem as Layout;

                var deletLayout = new DeleteLayout(viewModel.Layouts, $"{layout.Id}")
                {
                    Owner = App.Current.MainWindow
                };

                deletLayout.ShowDialog();

            }
        }


        private void TrvLayouts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            trvLayouts.UpdateLayout();

            if (trvLayouts.SelectedItem is Field)
            {
                var item = trvLayouts.SelectedItem as Field;
                var editField = new EditField(viewModel.Layouts, $"{item.Id}")
                {
                    Owner = App.Current.MainWindow
                };

                editField.ShowDialog();
            }
        }

        private void TrvLayouts_KeyDown_1(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                deleteItem();
            }
        }

        private void LstFiles_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if(viewModel.FileProcess.Count > 0)
                {
                    while (lstFiles.SelectedItems.Count > 0)
                    {
                        viewModel.FileProcess.RemoveAt(lstFiles.SelectedIndex);
                    }
                }
            }
        }
    }
}
