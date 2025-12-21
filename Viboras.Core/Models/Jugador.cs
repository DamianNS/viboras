using System;
using System.Collections.Generic;
using System.Text;

namespace Viboras.Core.Models
{
    public class Jugador
    {
        public Jugador(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }

        public CuerpoModel Cuerpo { get; set; } = new CuerpoModel();

        public DireccionEnum Direccion { get; set; } = DireccionEnum.Derecha;
        public int Largo { get; set; } = 5;
    }
}
