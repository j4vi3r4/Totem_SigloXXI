namespace Totem_SigloXXI.ViewModel
{

    //CLASE DONDE TODOS VIENEN 
    public class MainViewModel
    {
        //nombre del binding del xaml
        public ComensalesViewModel Comensales { get; set; }
        public MesasViewModel Mesas { get; set; }
        public DisponibilidadViewModel Disponibilidad { get; set; }



        public MainViewModel()
        {
            this.Comensales = new ComensalesViewModel();
            this.Mesas = new MesasViewModel();
            this.Disponibilidad = new DisponibilidadViewModel();
        }

       
    }
}
