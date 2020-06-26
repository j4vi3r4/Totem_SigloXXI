
namespace Totem_SigloXXI.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    //CLASE DONDE TODOS VIENEN 
    public class MainViewModel
    {
        //nombre del binding del xaml
        public ComensalesViewModel Comensales { get; set; }
        public MesasViewModel Mesas { get; set; }
        



        public MainViewModel()
        {
            this.Comensales = new ComensalesViewModel();
            this.Mesas = new MesasViewModel();            
        }

       


    }
}
