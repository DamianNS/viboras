using System;
using System.Collections.Generic;
using System.Text;

namespace Viboras.Core.Models
{
    public class CuerpoModel
    {
        public System.Drawing.Color Color { get; set; }
        public Queue<Bloque> Cola { get; set; } = new Queue<Bloque>();
    }
}
