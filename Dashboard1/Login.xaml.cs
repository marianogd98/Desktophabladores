using ConexApiRio;
using ConexApiRio.Service;
using ConexApiRio.Model;
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

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        JsonPlaceHolderApi jsonPlaceHolderApi;
        DataConfig dataConfig;
        public Login()
        {
            InitializeComponent();
            dataConfig = new DataConfig().GetDataConfig();
            if (dataConfig != null) {
                Uri resourceUri = new Uri(DefUrls.GetUrlImage("images/login.png"), UriKind.Absolute);
                logo.Source = new BitmapImage(resourceUri);
                jsonPlaceHolderApi = new JsonPlaceHolderApi();
                txtUser.Focus();
            }
            else
            {
                MessageBox.Show("Error Por favor Revise el archivo de Configuración, la aplicación se cerrará.", "Error Configuración", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion();           
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IniciarSesion();
            }
        }

        private void IniciarSesion()
        {
            if (txtUser.Text.Equals("") || txtPassword.Equals(""))
            {
                MessageBox.Show("Ingrese Usuario y Contraseña", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                /*Inside "0001",*/
                /*Playa el angel "0104",*/
                /*Juan Bautista "0101"*/
                /*Traki "0102"*/
                /*Centro Distribucion "0103"*/
                LoginUser loginUser = jsonPlaceHolderApi.Login(txtUser.Text, txtPassword.Password, dataConfig.Suc);
                if (loginUser.Result != null && loginUser.Result.CompareTo("off") != 0 && loginUser.Result.CompareTo("Terminal Bloqueado") != 0 && loginUser.Result.CompareTo("PcUnauthorized") != 0 && loginUser.Result.CompareTo("Unauthorized") != 0 && loginUser.Result.CompareTo("InternalServerError") != 0 && loginUser.Result.CompareTo("CuotaResponse") != 0)
                {
                    MainWindow mainWindow = new MainWindow(loginUser, dataConfig);
                    //ListView mainWindow = new ListView(loginUser);
                    mainWindow.Show();
                    this.Hide();
                }
                else
                {
                    try
                    {
                        if (loginUser.Result.CompareTo("Unauthorized") == 0)
                        {
                            MessageBox.Show("Error: Usuario y/o Contraseña invalida.", "Error Login", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (loginUser.Result.CompareTo("PcUnauthorized") == 0)
                        {
                            MessageBox.Show("Error: Equipo no Autorizado al sistema..", "Error Login", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        if (loginUser.Result.CompareTo("Terminal Bloqueado") == 0)
                        {
                            MessageBox.Show("Error: Este Terminal esta BLOQUEADO", "TERMINAL BLOQUEADO", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        if (loginUser.Result.CompareTo("CuotaResponse") == 0)
                        {
                            MessageBox.Show("Error: Cuota Respose Http limitada", "Sin Acceso", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        if (loginUser.Result.CompareTo("off") == 0)
                        {
                            MessageBox.Show("Error: No hay Conexión con el Servidor", "Error de Conexión", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error: No hay Conexión con el Servidor", "Error de Conexión", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
            }
        }
    }

}
