using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Totem_SigloXXI.Services;
using Xamarin.Forms;

namespace Totem_SigloXXI.ViewModel
{
    public class MesasViewModel : BaseViewModel
    {
        private ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<Mesa> mesas; //mesas en minusc private - atributo privado 

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

        public MesasViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMesas();
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
            var response = await this.apiService.GetList<Mesa>(url, "/app","/listarmesas"); //acá lista de mesa
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

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMesas);
            }
        }


    }
}
