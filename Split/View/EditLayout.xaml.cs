using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Split.Model;

namespace Split.View
{
    /// <summary>
    /// Lógica interna para EditLayout.xaml
    /// </summary>
    public partial class EditLayout : MetroWindow
    {
        public ObservableCollection<Layout> layout;
        public EditLayout(ObservableCollection<Layout> layouts)
        {
            layout = layouts;
            var vm = new ViewModel.VmLayout(this);
            this.DataContext = vm;
            InitializeComponent();
        }

        public async void msgSave(Layout layoutName)
        {

            await this.ShowMessageAsync("Criado com Sucesso!",
                                        $"Id: {layoutName.Id}" + Environment.NewLine +
                                        $"Layout: {layoutName.Name}" + Environment.NewLine +
                                        $"Modelo: {layoutName.File}" + Environment.NewLine +
                                        $"Heigth: {layoutName.Height}" + Environment.NewLine +
                                        $"Width: {layoutName.Width}");

            this.Close();

        }

        private void MetroWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            this.Close();
        }
    }
}
