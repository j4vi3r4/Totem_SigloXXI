using GalaSoft.MvvmLight.Command;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Totem_SigloXXI.Services;
using Xamarin.Forms;

namespace Totem_SigloXXI.ViewModel
{
    public class ComensalesViewModel : BaseViewModel
    {

        #region Attributes
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
            
            this.IsEnabled = true;
        }
        #endregion

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
        }
        #endregion




    }

}

