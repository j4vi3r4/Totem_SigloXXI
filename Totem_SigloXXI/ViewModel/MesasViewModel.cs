namespace Totem_SigloXXI.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Totem_SigloXXI.Services;
    using Totem_SigloXXI.Views;
    using Xamarin.Forms;
    public class MesasViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region Attributes
        
        private ApiService apiService;
        private bool isRefreshing;//para refrescar lista
        private bool isRunnning;//si esta corriendo
        private ObservableCollection<Mesa> mesas; //mesas en minusc private - atributo privado 
        private string _id_Mesa; //Debería contener mesa ID
        public string TxtCantComensal { get; set; } // Binding de la cantidad de personas para la mesa solicitada

        #endregion

        #region Properties

        public ObservableCollection<Mesa> Mesas //mesas mayus public - propiedad publica
        {
            get { return this.mesas;  } // devuelve el atributo privado de mesas 
            set { this.SetValue(ref this.mesas, value); } // se encarga de asignar y refrescar la viewmodel
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); } 
        }

       
        public bool IsRunnning
        {
            get { return this.isRunnning; }
            set { this.SetValue(ref this.isRunnning, value); }
        }

        public string Id_Mesa
        {
            get { return this._id_Mesa; }
            set { this.SetValue(ref this._id_Mesa, value); }
        }
        #endregion

        #region Constructors

        public MesasViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMesas();

            AsignarMesaCommand = new Command<string>(
                execute: (string arg) =>
                {
                    //Debug.WriteLine("------> Command " + arg);
                    ChangeMesaDisponibility(arg);
                    RefreshCanExecutes();
                }
                );

            RefreshCommand = new Command(
                execute: () => {
                    //Debug.WriteLine("------> Refresh");
                    LoadMesas();
                    RefreshCanExecutes();
                }
                );

            //For test purposes
            DigitCommand = new Command<string>(
                execute: (string arg) =>
                {
                   // Debug.WriteLine("------> Digit {0}");
                    RefreshCanExecutes();
                }
                );

            SolicitarMesaCommand = new Command(
                execute: () => {
                    Solicitar();
                    RefreshCanExecutes();
                }
                );

        }
        #endregion

        #region  Commands
        public ICommand RefreshCommand { private set; get; }
        public ICommand AsignarMesaCommand{ private set; get; }
        public ICommand DigitCommand { private set; get; }
        public ICommand SolicitarMesaCommand { private set; get; }

        void RefreshCanExecutes()
        {
            ((Command)RefreshCommand).ChangeCanExecute();
            ((Command)AsignarMesaCommand).ChangeCanExecute();
            ((Command)DigitCommand).ChangeCanExecute();
            ((Command)SolicitarMesaCommand).ChangeCanExecute();
        }

        /// <summary>
        /// Refresca la lista de mesas
        /// </summary>
        private async void LoadMesas()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection(); // validación de conexión a internet 
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var response = await this.apiService.GetList<Mesa>(url, prefix,"/listarmesas"); //acá lista de mesa
            
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message,"Accept");
                return;
            }

            var list = (List<Mesa>)response.Result;
            this.Mesas = new ObservableCollection<Mesa>(list);
            this.IsRefreshing = false; //para que no de vueltas el circulo de refresh 
        }

        /// <summary>
        /// Cambia la disponibilidad de la mesa a "No disponible"
        /// </summary>
        /// <param name="id_mesa">La ID de la mesa seleccionada</param>
        private async void ChangeMesaDisponibility(string id_mesa)
        {
            var connection = await this.apiService.CheckConnection(); // validación de conexión a internet 
            //Debug.WriteLine("------> ID_MESA {0}", id_mesa);
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }

            // Aquí pasamos truchamente el JSON como string
            // No me da el tiempo para poder hacer un método bonito :c
            string json = "{\"id_mesa\": \"" + id_mesa + "\"}";

            string url = Application.Current.Resources["UrlAPI"].ToString();
            string prefix = Application.Current.Resources["Prefix"].ToString();
            var response = await this.apiService.Post(url, prefix, "/cambiarDisponibilidadMesa", json); //acá lista de mesa

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            if (response.Result.ToString().Equals("0")) {
                await Application.Current.MainPage.DisplayAlert("Error", "Error al asignar mesa", "Accept");
                return;
            }

            GenerateComensal(id_mesa);

            await Application.Current.MainPage.DisplayAlert("OK", "Mesa asignada correctamente", "Accept");

            // Lleva a la página de inicio
            await Application.Current.MainPage.Navigation.PushAsync(new BienvenidosPage());
            return;

        }

        /// <summary>
        /// Obtiene la cantidad de comensales y pasa a la View de seleccionar mesa
        /// </summary>
        private async void Solicitar()
        {
            try
            {
                if (string.IsNullOrEmpty(this.TxtCantComensal))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar cantidad de personas", "Aceptar");
                    return;
                }

                var cantidad = int.Parse(this.TxtCantComensal);
                if (cantidad <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe ser superior a 0", "Aceptar");
                    return;
                }
                else if (cantidad >= 1)
                {
                    //prueba que se envia a la otra page (IDEA: CAPTURAR DATOS DEL ENTRY - > CAPTURAR LA DISPONIBILIDAD Y LA CANTIDAD DE LA MESA )
                    await Application.Current.MainPage.Navigation.PushAsync(new SeleccionMesaDiseno());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ser un número", "Aceptar");
                return;
            }

        }

        private async void GenerateComensal(string id_mesa) {
            var connection = await this.apiService.CheckConnection(); // validación de conexión a internet 
            
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }

            // Aquí pasamos truchamente el JSON como string
            // No me da el tiempo para poder hacer un método bonito :c
            string json = "{ " +
                               "\"cantidadPersonas\": \""+ TxtCantComensal + "\"," +
                               "\"id_mesa\" : \""+ id_mesa +"\", " +
                               "\"id_trabajador\":\"15\"" + //Usamos el 15 por que no creo que alcance a hacer algo para ver si hay trabajadores disponibles

                          "}";

            string url = Application.Current.Resources["UrlAPI"].ToString();
            string prefix = Application.Current.Resources["Prefix"].ToString();
            var response = await this.apiService.Post(url, prefix, "/createComensal", json); //acá lista de mesa

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

        }
    }
        #endregion
}
