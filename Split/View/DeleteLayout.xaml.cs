using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Split.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Split.View
{
    /// <summary>
    /// Lógica interna para DeleteLayout.xaml
    /// </summary>
    public partial class DeleteLayout : MetroWindow
    {
        public DeleteLayout(ObservableCollection<Layout> layouts, string id)
        {
            var vm = new ViewModel.VmDelete(layouts, id, this);
            this.DataContext = vm;

            InitializeComponent();

            lvDataBinding.ItemsSource = vm.Layout_Tmp.Fields;


        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        public async void msgDeleteLayout(String idLayout, string nameLayout, string alter)
        {

            await this.ShowMessageAsync($"Layout {alter} com Sucesso!",
                                        $"Id: {idLayout}" + Environment.NewLine +
                                        $"Name: {nameLayout}" + Environment.NewLine + Environment.NewLine);
            this.Close();

        }
    }
}
