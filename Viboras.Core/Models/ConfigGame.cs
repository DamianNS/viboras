using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Viboras.Core.Models
{
    public class ConfigGame
    {
        public int JugadoresMaximo { get; set; } = 1;

        public Color[] ColorJugador = new Color[]
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Purple,
            Color.Orange
        };
    }
}
