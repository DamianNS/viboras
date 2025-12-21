using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Viboras.Core.Models
{
    public class Bloque
    {
        public Point Point { get; set; }
        
        public TipoEnum Tipo { get; set; }

        public Color Color { get; set; }
    }
}
