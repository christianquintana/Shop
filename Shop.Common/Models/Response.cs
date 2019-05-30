using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
    public class Response
    {
        // Indicador de que el Api fue o no consumido 
        public bool IsSuccess { get; set; }

        // Mensaje si el Api no ha sido consumido
        public string Message { get; set; }

        // Devuelve el resultado del Api consumido 
        public object Result { get; set; }

    }
}
