using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Totem_SigloXXI.Services;
using Xamarin.Forms;

namespace Totem_SigloXXI.ViewModel
{
    public class MesasViewModel : BaseViewModel
    {
        private ApiService apiService;

        private ObservableCollection<Mesa> mesas; //mesas en minus private

        public ObservableCollection<Mesa> Mesas //mesas mayus public
        {
            get { return this.mesas;  } // devuelve el atributo privado de mesas 
            set { this.SetValue(ref this.mesas, value); } // se encarga de asignar y refrescar la viewmodel
        }

        public MesasViewModel()
        {
            this.apiService = new ApiService();
            this.LoadMesas();
        }

        private async void LoadMesas()
        {
            var response = await this.apiService.GetList<Mesa>("http://51.143.4.69:4567","/app","/listarmesas"); //acá lista de mesa
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message,"Accept");
                return;
            }

            var list = (List<Mesa>)response.Result;
            this.Mesas = new ObservableCollection<Mesa>(list);
        }
    }
}
