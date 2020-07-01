namespace Totem_SigloXXI.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    //Clase si hay o no conexión 
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result {get; set; }
    }
}
