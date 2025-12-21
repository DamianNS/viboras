using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Viboras.Core;
using Viboras.Core.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Viboras.Components.Pages
{
    public class JuegoPage: ComponentBase, IDisposable
    {
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Parameter] public string PartidaId { get; set; } = "";

        protected const int GridSize = 50;
        protected const int PixelSize = 6;
        protected int IntervalMs = 1000;

        private GameCore Game;
        protected List<Jugador> Jugadores = new();

        protected Shared.CanvasComponent? canvasComponent;

        protected override void OnInitialized()
        {
            var config = new ConfigGame
            {
                JugadoresMaximo = 1,
                MapHorizontal = GridSize,
                MapVertical = GridSize,
            };
            Game = new GameCore(config);
            var jugador = Game.AgregarJugador("Jugador1");
            Jugadores.Add(jugador);

            Game.ActulizarMapa += async (object? s, Bloque bloque) =>
            {
                // When map updates, draw the single block on canvas
                try
                {
                    //if (canvasComponent is not null)
                    {
                        var color = bloque.Color.Name;
                        var cell = new Shared.CanvasComponent.CellData(bloque.Point.X, bloque.Point.Y, color);
                        await canvasComponent?.DrawCellsAsync([cell]);
                    }
                }
                catch
                {
                    // ignore drawing errors
                }

                //await InvokeAsync(StateHasChanged);
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && canvasComponent is not null)
            {
                // draw grid and initial map
                await canvasComponent.DrawGridAsync(GridSize, GridSize);

                // Draw initial occupied cells from jugadores
                var cells = new List<Shared.CanvasComponent.CellData>();
                foreach (var j in Jugadores)
                {
                    foreach (var b in j.Cuerpo.Cola)
                    {
                        var color = j.Cuerpo.Color.Name;
                        cells.Add(new Shared.CanvasComponent.CellData(b.Point.X, b.Point.Y, color));
                    }
                }
                if (cells.Any()) await canvasComponent.DrawCellsAsync(cells);

                // start the game loop
                _ = StartLoop();
            }
        }

        private async Task StartLoop()
        {
            _ = Game.Start();
        }

        protected void SetDirection(DireccionEnum dir)
        {
            // prevent immediate reverse
            var current = Jugadores.First().Direccion;
            if ((current == DireccionEnum.Izquierda && dir == DireccionEnum.Derecha) ||
                (current == DireccionEnum.Derecha && dir == DireccionEnum.Izquierda) ||
                (current == DireccionEnum.Arriba && dir == DireccionEnum.Abajo) ||
                (current == DireccionEnum.Abajo && dir == DireccionEnum.Arriba))
                return;
            Jugadores.First().Direccion = dir;
        }

        protected void HandleKey(KeyboardEventArgs e)
        {
            if (e.Key == "ArrowUp") 
                SetDirection(DireccionEnum.Arriba);
            else if (e.Key == "ArrowDown") SetDirection(DireccionEnum.Abajo);
            else if (e.Key == "ArrowLeft") SetDirection(DireccionEnum.Izquierda);
            else if (e.Key == "ArrowRight") SetDirection(DireccionEnum.Derecha);
        }

        protected void ResetGame()
        {
            Game.Reset();
            // clear canvas
            _ = canvasComponent?.ClearAsync();
        }

        protected void VolverAlLobby()
        {
            Navigation.NavigateTo("/lobby");
        }

        protected int UserSnakeLength => Jugadores.FirstOrDefault()?.Cuerpo.Cola.Count ?? 0;

        public void Dispose()
        {
            Game.Reset();
        }
    }
}
