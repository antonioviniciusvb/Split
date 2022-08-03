using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Split.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Split.View
{
    /// <summary>
    /// Lógica interna para DeleteField.xaml
    /// </summary>
    public partial class DeleteField : MetroWindow
    {
        public DeleteField(ObservableCollection<Layout> layouts, string id)
        {
            var vm = new ViewModel.VmDelete(layouts, id, this);
            this.DataContext = vm;

            InitializeComponent();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public async void msgDeleteField(Field field, string nameLayout, string alter)
        {

            await this.ShowMessageAsync($"Campo foi {alter} com Sucesso!",
                                        $"Layout: {nameLayout}" + Environment.NewLine + Environment.NewLine +
                                        $"     Id: {field.Id}" + Environment.NewLine +
                                        $"     Nome: {field.Name}" + Environment.NewLine +
                                        $"     Tipo: {field.Type}" + Environment.NewLine +
                                        $"     Exibição: {field.Display}" + Environment.NewLine +
                                        $"     Posição X: {field.Position_x}" + Environment.NewLine +
                                        $"     Posição Y: {field.Position_y}" + Environment.NewLine +
                                        $"     Rotação: {field.Rotation}" + Environment.NewLine +
                                        $"     Font: {field.Font.FontString}" + Environment.NewLine +
                                        $"     Expressão: {field.Expression}" + Environment.NewLine);
            this.Close();

        }


        

        public async void msgQuestion(string nameLayout)
        {
            await this.ShowMessageAsync($"Realmente deseja Excluir o Layout: {nameLayout} ?",
                                   $"", MessageDialogStyle.AffirmativeAndNegative);
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }
    }
}
