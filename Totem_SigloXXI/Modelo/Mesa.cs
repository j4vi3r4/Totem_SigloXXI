﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Totem_SigloXXI
{
    public class Mesa
    {        
        //Clase con atributos 
      
        public int IdMesa { get; set; }                
        public int Numero { get; set; }        
        public int Capacidad { get; set; }
        public string Disponibilidad { get; set; }


        //
        public override string ToString()
        {
            return this.Disponibilidad;                   
        }
    }
}
