namespace Totem_SigloXXI.ViewModel
{
    using Totem_SigloXXI.Modelo;
    using Totem_SigloXXI.Services;
    using Xamarin.Forms;

    public class DisponibilidadViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isEnabled; //para botón 
      //  private Mesa id_Mesa;
        private bool isRunnning;
        

        #endregion

        #region Properties
        public string TxtCantComensal { get; set; }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public int IdMesa { get; set; }
        //     public string Disponibilidad { get; set; }
        #endregion

        //no se si los ocupe todos 
        public string CantidadPersonas { get; set; }
        public string Id_Mesa { get; set; }
        public string Id_Trabajador { get; set; }

        #region MyRegion
        public DisponibilidadViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            //this.AsignarMesa();
        }
        #endregion

        #region MyRegion

        public bool IsRefreshing { get; private set; }

        //asigna una mesa 
        /*{
            "id_mesa": "3"
            }*/
        /*{
            "cantidadPersonas": "2",
            "id_mesa" : "1",
            "id_trabajador":"15"
           }
        */
        //binding de bienvenidos TxtCantComensal -> cantidad de comensales 
        private async void AsignarMesa()
        {
            this.isRunnning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection(); // validación de conexión a internet 
            if (!connection.IsSuccess)
            {
                this.isRunnning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }
            var mesa = new Mesa
            {
                Id_mesa = this.IdMesa,
            };
            var comensal = new Comensal
            {
                CantidadPersonas = this.CantidadPersonas,

            };
      
            //asigna una mesa 
            /*{
             "id_mesa": "3"
             }*/
            /*{
              "cantidadPersonas": "2",
                 "id_mesa" : "1",
               "id_trabajador":"15"
               }
            */
            //var cambiarDisponibilidad = this.id_Mesa;
            //enviar en 0? enviar información 
           /* var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var response = await this.apiService.Post(url, prefix, "/cambiarDisponibilidadMesa", mesa);
            if (!response.IsSuccess)
            {
                this.isRunnning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            this.isRunnning = false;
            this.IsEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();*/

            #endregion
        }

    }
}
