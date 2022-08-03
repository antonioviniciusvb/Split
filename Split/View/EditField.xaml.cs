using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Split.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Split.View
{
    /// <summary>
    /// Lógica interna para Teste.xaml
    /// </summary>
    public partial class EditField : MetroWindow
    {

        public EditField(ObservableCollection<Layout> layouts, string id)
        {
            InitializeComponent();

            var vm = new ViewModel.VmField(layouts, id, this);
            this.DataContext = vm;

            

            //List<string> rotation = new List<string> { "0", "90", "180", "270" };
            //List<string> field_Display = new List<string> { "Todas", "Ímpares", "Pares" };
            List<string> sepators = new List<string> { "/", ".", "_", "none" };
            List<string> sepatorsTime = new List<string> { ":", ".", "_", "none" };
            List<string> counts = new List<string> { "Página", "Campo", "Arquivo Split" };
            List<string> orders = new List<string> { "Dia Mês Ano", "Ano Mês Dia", "Mês Dia Ano" };
            List<string> ordersTime = new List<string> { "Horas Minutos Segundos", "Horas Minutos"};
            List<string> sensitiveCase = new List<string> { "Normal", "Lower", "Upper" };
            List<string> logicVariables = new List<string> { "Data", "Hora", "Arquivo", "Contador" };
            List<string> fonts = new List<string> { "Arial", "Courier New", "Times New Roman" };

            nX.Minimum = nY.Minimum = 6;

            //Date
            cbDateOrder.ItemsSource = orders;
            cbSeparator.ItemsSource = sepators;

            //Counter
            cbCounter.ItemsSource = counts;

            //File
            cbSensitiveCase.ItemsSource = sensitiveCase;

            //Time
            cbSeparatorTime.ItemsSource = sepatorsTime;
            cbTimeOrder.ItemsSource = ordersTime;

            cbVariablesLogic.ItemsSource = logicVariables;
            cbFont.ItemsSource = fonts;
        }

        public async void msgSaveField(Field field, string nameLayout, string alter)
        {

            await this.ShowMessageAsync($"Campo foi {alter} ao Layout com Sucesso!",
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

        private void MetroWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }
    }
}
