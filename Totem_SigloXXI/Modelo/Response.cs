using System;
using System.Collections.Generic;
using System.Text;

namespace Totem_SigloXXI.Modelo
{
    //clase si hay o no conexión 
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result {get; set; }

    }
}
