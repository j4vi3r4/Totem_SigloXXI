namespace Totem_SigloXXI.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Totem_SigloXXI.Services;
    using Xamarin.Forms;
    public class ComensalesViewModel : BaseViewModel
    {

        #region Attributes
        private ApiService apiService;
        private bool isRunnning;
        private bool isEnabled;
        private string _id_Mesa;
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

        public string Id_Mesa
        {
            get { return this._id_Mesa; }
            set { this.SetValue(ref this._id_Mesa, value); }
        }
        #endregion

        #region Constructors
        public ComensalesViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            SolicitarMesaCommand = new Command(
                execute: () => {
                    Solicitar();
                    RefreshCanExecutes();
                }
                );
        }
        #endregion
        //si hay menos de la que se necesitan llamar al administrador keys (8)
        //cant. mesa disponibilidad
        // 1 mesa tiene 4 sillas 
        /* if mesa cant 2 == 2 sillas, cant 4 == 4sillas, 6 = 6 sillas*/
        #region Commands
        public ICommand SolicitarMesaCommand { get; set; }
        void RefreshCanExecutes()
        {
            ((Command)SolicitarMesaCommand).ChangeCanExecute();
        }

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
                else if ( cantidad >= 1)
                {
                    //prueba que se envia a la otra page (IDEA: CAPTURAR DATOS DEL ENTRY - > CAPTURAR LA DISPONIBILIDAD Y LA CANTIDAD DE LA MESA )
                    await Application.Current.MainPage.Navigation.PushAsync(new SeleccionMesaDiseno());
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ser un número", "Aceptar");
                return;
            }

        }
        #endregion
    }
}

