using System.Drawing;
using Viboras.Core.Models;

namespace Viboras.Core
{
    public class GameCore
    {
        private List<Jugador> jugadores = new List<Jugador>();
        private readonly ConfigGame config;
        private readonly Bloque[,] Mapa = new Bloque[100,100];

        public EventHandler<Bloque> ActulizarMapa;

        public GameCore(ConfigGame config)
        {
            this.config = config;
            InicializaMapa();
        }

        public Jugador AgregarJugador(string nombre)
        {
            if(jugadores.Count >= config.JugadoresMaximo)
            {
                throw new InvalidOperationException("No se pueden agregar más jugadores.");
            }
            var j = new Jugador(nombre);
            j.Direccion = DireccionEnum.Derecha;
            j.Cuerpo.Color = config.ColorJugador[jugadores.Count];
            var posicionInicial = new Point(10,10 + jugadores.Count * 10);
            Bloque bloqueInicial = new Bloque
            {
                Point = posicionInicial,
                Tipo = TipoEnum.Cuerpo,                
            };
            j.Cuerpo.Cola.Enqueue(bloqueInicial);
            jugadores.Add(j);
            return j;
        }

        public async Task Start()
        {
            bool gameOver = false;
            
            while (!gameOver)
            {
                var inicio = DateTime.Now;
                foreach (var jugador in jugadores)
                {
                    var cabeza = jugador.Cuerpo.Cola.Last();
                    var nuevaCabeza = new Bloque
                    {                      
                        Tipo = TipoEnum.Cabeza,
                        Color = jugador.Cuerpo.Color
                    };
                    // Lógica del turno del jugador
                    switch (jugador.Direccion)
                    {
                        case DireccionEnum.Derecha:
                            nuevaCabeza.Point = new Point(cabeza.Point.X + 1, cabeza.Point.Y);

                            // Validacion limites del mapa
                            if (nuevaCabeza.Point.X >= Mapa.GetLength(0))
                            {
                                gameOver = true;
                                Console.WriteLine($"Jugador {jugador.Nombre} ha chocado contra la pared derecha. Fin del juego.");
                                continue;
                            }
                            break;
                        case DireccionEnum.Arriba:
                            break;
                        case DireccionEnum.Abajo:
                            break;
                        case DireccionEnum.Izquierda:
                            break;
                    }
                                            
                    jugador.Cuerpo.Cola.Enqueue(nuevaCabeza);
                    Mapa[nuevaCabeza.Point.X, nuevaCabeza.Point.Y] = nuevaCabeza;
                    ActulizarMapa?.Invoke(this, nuevaCabeza);
                    // Remover la cola para simular movimiento
                    if (jugador.Largo <= jugador.Cuerpo.Cola.Count)
                    {
                        var borrar = jugador.Cuerpo.Cola.Dequeue();
                        Mapa[borrar.Point.X, borrar.Point.Y].Tipo = TipoEnum.Vacio;
                        ActulizarMapa?.Invoke(this, borrar);
                    }                    
                }

                // Calculo de demora para mantener un turno de 1 segundo
                var fin = DateTime.Now;
                var duracionTurno = fin - inicio;
                Console.WriteLine($"Duración del turno: {duracionTurno.TotalMilliseconds} ms");
                var espera = 1000 - duracionTurno.TotalMilliseconds;
                if (espera > 0)
                {
                    await Task.Delay((int)espera);
                }
            }
        }

        private void InicializaMapa()
        {
            for(int x = 0; x < 100; x++)
            {
                for(int y = 0; y < 100; y++)
                {
                    Mapa[x,y] = new Bloque { Tipo = TipoEnum.Vacio };
                }
            }
        }
    }
}
