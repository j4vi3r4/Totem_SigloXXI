namespace Totem_SigloXXI.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Totem_SigloXXI.Services;
    using Xamarin.Forms;
    public class ComensalesViewModel : BaseViewModel
    {

        #region Attributes
        private ApiService apiService;
        private bool isRunnning;
        private bool isEnabled;
        #endregion


        #region Properties
        public string TxtCantComensal { get; set; }

        public bool IsRunnning {
            get { return this.isRunnning; }
            set { this.SetValue(ref this.isRunnning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructors
        public ComensalesViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
        #endregion
        //si hay menos de la que se necesitan llamar al administrador keys (8)
        //cant. mesa disponibilidad
        // 1 mesa tiene 4 sillas 
        /* if mesa cant 2 == 2 sillas, cant 4 == 4sillas, 6 = 6 sillas*/
        #region Commands
        public ICommand SolicitarMesaCommand { get { return new RelayCommand(Solicitar); }  }

        private async void Solicitar()
        {
                                                                                                                                           
            if (string.IsNullOrEmpty(this.TxtCantComensal))
            {
                await Application.Current.MainPage.DisplayAlert("Error","Debe ingresar cantidad de personas","Aceptar");
                return;
            }

            var cantidad = int.Parse(this.TxtCantComensal);
            if (cantidad <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ser superior a 0", "Aceptar");
                return;
            }
            else if (cantidad <=1 | cantidad >=2){
                //prueba que se envia a la otra page (IDEA: CAPTURAR DATOS DEL ENTRY - > CAPTURAR LA DISPONIBILIDAD Y LA CANTIDAD DE LA MESA )
                await Application.Current.MainPage.Navigation.PushAsync(new SeleccionMesaDiseno());
            }

        }
        #endregion
    }
}

