using ConexApiRio.Model;
using ConexApiRio.Service;
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
using System.Windows.Threading;


namespace Dashboard1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        JsonPlaceHolderApi jsonPlaceHolderApi;
        LoginUser userConect;
        PrintSvc printSvc;
        private DispatcherTimer RefreshTimer = new DispatcherTimer();
        private DataConfig _dataConfig;
        public MainWindow(LoginUser loginUser, DataConfig dataConfig)
        {
            InitializeComponent();
            
            if (dataConfig != null)
            {
                _dataConfig = dataConfig;
                jsonPlaceHolderApi = new JsonPlaceHolderApi(dataConfig.Dpto);                     
                printSvc = new PrintSvc();
                RefreshHabladores();
                userConect = loginUser;
            }
            else
            {
                MessageBox.Show("Error Por favor Revise el archivo de Configuración, la aplicación se cerrará.","Error Configuración",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void RefreshHabladores()
        {
            txtBuscar.Clear();
            txtBuscar.Focus();
            dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
        }

        private void PreloadHabladores()
        {
            // Install a timer to show each image.
            RefreshTimer.Interval = TimeSpan.FromMinutes(30);//FromSeconds(5);
            RefreshTimer.Tick += Tick;
            RefreshTimer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            RefreshHabladores();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        bool esNumero(string value)
        {
            long outvar = 0;
            return Int64.TryParse(txtBuscar.Text,out outvar);
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
                //dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHablador(txtBuscar.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
       
            var res = ProductoService.GetProduct(txtBuscarProd.Text);
            if(res.success == 1 && res.data.id != -1)
            {
                txtbDesc.Text = string.IsNullOrEmpty(res.data.descripcion) ? "Nan" : res.data.descripcion;
                txtbCodP.Text = string.IsNullOrEmpty(res.data.codigoProducto) ? "Nan" : res.data.codigoProducto;
                txtbSubg.Text = res.data.codigoSubgrupo == null ? "Nan" : res.data.codigoSubgrupo.ToString();
                txtbPeso.Text = res.data.pesado.ToString();
                txtbStat.Text = res.data.estatus.ToString();
                txtbReg.Text = res.data.regulado.ToString();
                txtbPrecD.Text = res.data.precioDetal.ToString();
                txtbCant.Text = res.data.cantidad.ToString();
                txtbPrecOf.Text = res.data.precioOferta.ToString();
                txtbCodB.Text = string.IsNullOrEmpty(res.data.barra) ? "Nan" : res.data.barra;
            }
            else
            {
                alertbDesc.Text = "Aviso:";
                txtbDesc.Text = "No hay informacion sobre este producto";
                txtbCodP.Text = "";
                txtbSubg.Text = "";
                txtbPeso.Text = "";
                txtbStat.Text = "";
                txtbReg.Text = "";
                txtbPrecD.Text = "";
                txtbCant.Text = "";
                txtbPrecOf.Text = "";
                txtbCodB.Text = "";
            }
            //if (!txtBuscar.Text.Equals(""))
            //    FiltrarDatosDatagrid(txtBuscar.Text);
            //else
            //   dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!txtBuscar.Text.Equals(""))
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
                if (mio.PorImprimir == 0)
                {
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
           /* var row = (CheckBox)sender;
            for (int i = 0; i < dgHabladores.Items.Count; i++)
            {
                // var rowView = (Habladores)dgHabladores.Items[i]; //Get RowView
                //rowView.PorImprimir = 1;
                //dgHabladores.Items.CurrentItem.// = (ItemCollection) rowView;
                //rowView.BeginEdit();
                //rowView[0] = row.IsChecked;
                //rowView.EndEdit();
                //dgHabladores.Items.Refresh(); // Refresh table
            }*/

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
            if (chkTodos.IsChecked == true)
            {
                for (int i = 0; i < dgHabladores.Items.Count; i++)
                {
                    var hablador = (Habladores)dgHabladores.Items[i]; //Get RowView
                    ImpresionMasiva(hablador);
                    Task.Delay(1000).Wait();
                }
                chkTodos.IsChecked = false;
            
                MessageBox.Show("Habladores Impresos y Actualizados", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                txtBuscar.Text = "";
                txtBuscar.Focus();
            }
        }

        private void BtnActualizaTodo_Click(object sender, RoutedEventArgs e)
        {
            if (chkActTodos.IsChecked == true)
            {
                if (MessageBox.Show("Esta seguro de Actualizar sin Imprimir?", "Actualizar Habladores", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < dgHabladores.Items.Count; i++)
                    {
                        var hablador = (Habladores)dgHabladores.Items[i]; //Get RowView
                        if (hablador.PorImprimir == 0)
                        {
                            string respuesta = jsonPlaceHolderApi.SetActualizarHabladorSinImpresion(hablador.Codigo, userConect.getCodigoUser(), userConect.Result);
                        }
                        Task.Delay(200).Wait();
                    }
                
                    chkActTodos.IsChecked = false;            
                    dgHabladores.ItemsSource = jsonPlaceHolderApi.GetHabladores();
                    MessageBox.Show("Habladores Actualizados","Mensaje",MessageBoxButton.OK,MessageBoxImage.Information);
                    txtBuscar.Text = "";
                }
            }
        }

        private void ChkActTodos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {   
            if (MessageBox.Show("Desea salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }                                   
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void RefreshHabladores_Click(object sender, RoutedEventArgs e)
        {
            RefreshTimer.Stop();
            RefreshHabladores();
            RefreshTimer.Start();
        }

        public void BuscarOfertas()
        { //, StringFormat=d <- esto es de data binding por imprimir
            txtBuscar.Clear();
            txtBuscar.Focus();
            var data = jsonPlaceHolderApi.GetHabladores().Where(h => h.Oferta == 1).ToList();
            List<HabladoresViewModel> lhdvm = new List<HabladoresViewModel>();
            data.ForEach( d => {
                string porImprimir = (d.PorImprimir == 0) ? "No Impreso" : "Ya fue Impreso";
                porImprimir += " - " + ((d.Oferta == 1) ? "Producto con precio de Oferta: " + d.Precioo : "");
                var obj = new HabladoresViewModel{
                       Codigo = d.Codigo,
                       Codigo_barra = d.Codigo_barra,
                       Departamento = d.Departamento,
                       Descripcion = d.Descripcion,
                       Fecha = d.Fecha,
                       FechHorImpresion = Convert.ToDateTime(d.FechHorImpresion).ToString(@"dd/MM/yyyy HH\:mm"),
                       Oferta = d.Oferta,
                       PorImprimir = porImprimir,
                       Precio = d.Precio,
                       Precioo = d.Precioo
                    };
                lhdvm.Add(obj);
            });
            dgHabladores.ItemsSource = lhdvm;// jsonPlaceHolderApi.GetHabladores().Where(h => h.Oferta == 1).ToList();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            BuscarOfertas();
        }

        public void BuscarProductosBimoneda()
        {
            txtBuscar.Clear();
            txtBuscar.Focus();
            try
            {
                var data = jsonPlaceHolderApi.GetHabladores("001");

                if (data != null)
                {
                    data = data.ToList();
                    List<HabladoresViewModel> lhdvm = new List<HabladoresViewModel>();
                    data.ForEach(d =>
                    {
                        string porImprimir = (d.Moneda.Contains("0000000001")) ? "Producto de Costo en Bolivares" : "Producto de Costo en Dolares";
                        porImprimir += " - " + ((d.Oferta == 1) ? "Producto con precio de Oferta: " + d.Precioo : "");
                        var obj = new HabladoresViewModel
                        {
                            Codigo = d.Codigo,
                            Codigo_barra = d.Codigo_barra,
                            Departamento = d.Departamento,
                            Descripcion = d.Descripcion,
                            Fecha = d.Fecha,
                            FechHorImpresion =  Convert.ToDateTime(d.FechHorImpresion).ToString(@"dd/MM/yyyy HH\:mm"),
                            Oferta = d.Oferta,
                            PorImprimir = porImprimir,
                            Precio = d.Precio,
                            Precioo = d.Precioo
                        };
                        lhdvm.Add(obj);
                    });
                    dgHabladores.ItemsSource = lhdvm;// jsonPlaceHolderApi.GetHabladores().Where(h => h.Oferta == 1).ToList();
                }
                else
                {
                    MessageBox.Show("No Existen Productos con Precio en Bs.", "Información de Producto", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgHabladores.Items.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error en carga de productos", "Error en servidor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Bimoneda(object sender, RoutedEventArgs e)
        {
            BuscarProductosBimoneda();
        }

        private void dgHabladores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkTodos_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtbDesc_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }    
}
