
namespace Totem_SigloXXI.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    //CLASE DONDE TODOS VIENEN 
    public class MainViewModel
    {
        public MesasViewModel Mesas { get; set; }

        public MainViewModel()
        {
            this.Mesas = new MesasViewModel();
           
        }

    }
}
