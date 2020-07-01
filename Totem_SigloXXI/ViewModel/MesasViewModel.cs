namespace Totem_SigloXXI.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;
    using Totem_SigloXXI.Modelo;
    using Totem_SigloXXI.Services;
    using Xamarin.Forms;
    public class MesasViewModel : BaseViewModel
    {
        #region Attributes
        
        private ApiService apiService;
        private bool isRefreshing;//para refrescar lista
        private bool isRunnning;//si esta corriendo
        private ObservableCollection<Mesa> mesas; //mesas en minusc private - atributo privado 
        
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
        #endregion

        #region Constructors

        public MesasViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMesas();
                     
        }
        #endregion

        #region  Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMesas);
            }
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

        //refrezca lista en la view
        
        //captura el comand de la view /button
        
        }
        #endregion
}
