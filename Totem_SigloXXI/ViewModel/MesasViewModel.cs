namespace Totem_SigloXXI.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Input;
    using Totem_SigloXXI.Services;
    using Xamarin.Forms;
    public class MesasViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region Attributes
        
        private ApiService apiService;
        private bool isRefreshing;//para refrescar lista
        private bool isRunnning;//si esta corriendo
        private ObservableCollection<Mesa> mesas; //mesas en minusc private - atributo privado 
        private string _id_Mesa; //Debería contener mesa ID

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
                    Debug.WriteLine("------> Command " + arg);
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

        }
        #endregion

        #region  Commands
        public ICommand RefreshCommand { private set; get; }
        public ICommand AsignarMesaCommand{ private set; get; }
        public ICommand DigitCommand { private set; get; }

        void RefreshCanExecutes()
        {
            ((Command)RefreshCommand).ChangeCanExecute();
            ((Command)AsignarMesaCommand).ChangeCanExecute();
            ((Command)DigitCommand).ChangeCanExecute();
        }


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

        private void ChangeMesaDisponibility(string id_mesa)
        {
            this.IsRefreshing = false;
            var connection = this.apiService.CheckConnection(); // validación de conexión a internet 
            Debug.WriteLine("------> ID_MESA {0}", id_mesa);
        }

        //refrezca lista en la view

        //captura el comand de la view /button

    }
        #endregion
}
