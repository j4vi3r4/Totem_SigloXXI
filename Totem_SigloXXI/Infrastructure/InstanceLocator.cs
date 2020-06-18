namespace Totem_SigloXXI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Totem_SigloXXI.ViewModel;

    public class InstanceLocator //INSTANCIANDO SOLO 1 INSTANCIA DE LA MAINVIEWMODEL
    {
        public MainViewModel Main { get; set; } //objeto que todas las paginas van a bindear

        public InstanceLocator()
        {
            this.Main = new MainViewModel(); // NUEVA INSTANCIA DE MAINVIEWMODEL
        }
    }
}
