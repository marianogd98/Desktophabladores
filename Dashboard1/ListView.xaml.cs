using ConexApiRio.Model;
using ConexApiRio.Service;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        JsonPlaceHolderApi jsonPlaceHolderApi;
        LoginUser userConect;
        PrintSvc printSvc;
        public ListView(LoginUser loginUser)
        {
            InitializeComponent();
            jsonPlaceHolderApi = new JsonPlaceHolderApi();
            printSvc = new PrintSvc();
            dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
            userConect = loginUser;
        }
        bool esNumero(string value)
        {
            int outvar = 0;
            return Int32.TryParse(txtBuscar.Text, out outvar);
        }
        private void FiltrarDatosDatagrid(string txt_buscar)
        {
            ///Al texto recibido si contiene un asterisco (*) lo reemplazo de la cadena
            ///para que no provoque una excepción.
            //string cadena = txt_buscar.Text.Trim().Replace("*", "");
            
            if (esNumero(txt_buscar))
            {
                dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHablador(txt_buscar);
            }
            else
            {
                dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladoresDescripcion(txt_buscar);
            }
        }
        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBuscar.Text.Equals(""))
            {
                dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
            }
            else            
            {
                dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHablador(txtBuscar.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!txtBuscar.Text.Equals(""))
                FiltrarDatosDatagrid(txtBuscar.Text);
            else
                dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(!txtBuscar.Text.Equals(""))
                  FiltrarDatosDatagrid(txtBuscar.Text);
            }
        }

        private void DgHabladores_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = (DataGridRow)sender;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Collapsed ?
            Visibility.Visible : Visibility.Collapsed;
        }

        private void PrintHablador(object sender, RoutedEventArgs e)
        {
            Habladores mio = (Habladores)(sender as Button).DataContext;
            //model.DynamicText = (new Random().Next(0, 100).ToString());           
            bool r = printSvc.SendTextFileToPrinter(mio);
            if (r)
            {
                if (mio.PorImprimir == 0) { 
                    dgHabladores.ItemsSource = jsonPlaceHolderApi.SetActualizarHablador(mio.Codigo, userConect.getCodigoUser(), userConect.Result);                    
                }
                if (!txtBuscar.Text.Equals(""))
                {
                    FiltrarDatosDatagrid(txtBuscar.Text);
                }
            }
            //printSvc.ImprimirHablador();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            //(sender as DataGrid).RowEditEnding -= dgHabladores_RowEditEnding;
            var row = (CheckBox)sender;
            for (int i = 0; i < dgHabladores.Items.Count; i++)
            {
                // var rowView = (Habladores)dgHabladores.Items[i]; //Get RowView
                //rowView.PorImprimir = 1;
                //dgHabladores.Items.CurrentItem.// = (ItemCollection) rowView;
                //rowView.BeginEdit();
                //rowView[0] = row.IsChecked;
                //rowView.EndEdit();
                //dgHabladores.Items.Refresh(); // Refresh table
            }

            //(sender as DataGrid).RowEditEnding += DataGrid_RowEditEnding;


            //DataGridViewCheckBoxColumn col1 = new DataGridViewCheckBoxColumn();
            /* for (int i = 0; i < dgHabladores.Items.Count; i++)
             {
                 Habladores check = (Habladores)dgHabladores.Items[i];
                 dgHabladores.Columns[0].SetValue(,true);
                 if (arr[i] == true)
                 {
                     check.Value = check.TrueValue;
                 }
             }*/
        }

        void ImpresionMasiva(Habladores hablador)
        {
            bool r = printSvc.SendTextFileToPrinter(hablador);
            if (r)
            {
                if (hablador.PorImprimir == 0)
                {
                    dgHabladores.ItemsSource = jsonPlaceHolderApi.SetActualizarHablador(hablador.Codigo, userConect.getCodigoUser(), userConect.Result);
                }
            }
        }

        private void BtnImprimirTodo_Click(object sender, RoutedEventArgs e)
        {
            if (chkTodos.IsChecked==true) {
                for (int i = 0; i < 5/*dgHabladores.Items.Count*/; i++)
                {
                    var hablador = (Habladores)dgHabladores.Items[i]; //Get RowView
                    ImpresionMasiva(hablador);
                    Task.Delay(1000).Wait();
                }
                chkTodos.IsChecked = false;
            }
            txtBuscar.Text = "";
        }
    }

    public enum OrderStatus { None, New, Processing, Shipped, Received };

    //Converts the mailto uri to a string with just the customer alias
    public class EmailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string email = value.ToString();
                int index = email.IndexOf("@");
                string alias = email.Substring(7, index - 7);
                return alias;
            }
            else
            {
                string email = "";
                return email;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Uri email = new Uri((string)value);
            return email;
        }
    }
}
