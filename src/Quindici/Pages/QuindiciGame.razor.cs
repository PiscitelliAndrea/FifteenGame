using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quindici.Data;

namespace Quindici.Pages
{
    public partial class QuindiciGame
    {
        #region Injection

        [Inject]
        TilesGenerator Tiles { get; set; }

        #endregion

        #region Internal properties

        protected Timer timer { get; set; }
        protected bool TimerStarted = false;

        protected List<Tile> tiles;
        protected Tile tile;

        //private int ElapsedTime { get; set; }
        private TimeSpan ElapsedTime = TimeSpan.FromMilliseconds(0);

        #endregion

        #region Life Cycle events

        protected override void OnInitialized()
        {
            tiles = Tiles.GenerateTiles();
        }

        #endregion

        #region Tiles Methods

        protected async Task ClickTile(int riga, int colonna)
        {
            if (!Tiles.Done)
            {
                if (!TimerStarted)
                {
                    TimerStarted = true;
                    StartCounter();
                }

                tiles = Tiles.TryMoveTile(riga, colonna);
                if (Tiles.Done)
                    StopCounter();
                this.StateHasChanged();
            }
        }

        protected void Restart()
        {
            ResetCounter();
            TimerStarted = true;
            StartCounter();
            tiles = Tiles.Restart();
            this.StateHasChanged();
        }

        #endregion

        #region Timer Methods

        void StartCounter()
        {
            timer = new Timer(new TimerCallback(_ =>
            {
                //ElapsedTime++;
                ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));

                // Note that the following line is necessary because otherwise
                // Blazor would not recognize the state change and not refresh the UI
                InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            }), null, 1000, 1000);
        }

        void ResetCounter()
        {
            ElapsedTime = new TimeSpan(0, 0, 0);
        }

        void StopCounter()
        {
            timer.Dispose();
        }

        #endregion
    }
}