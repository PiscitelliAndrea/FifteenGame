using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Quindici.Data;

namespace Quindici.Pages
{
    public partial class QuindiciGame
    {
        [Inject]
        TilesGenerator Tiles { get; set; }

        protected List<Tile> tiles;
        protected Tile tile;

        protected override void OnInitialized()
        {
            tiles = Tiles.GenerateTiles();
        }

        protected void ClickTile(int riga, int colonna)
        {
            if (!Tiles.Done)
            {
                tiles = Tiles.TryMoveTile(riga, colonna);
                RefreshPage();
            }
        }

        protected void Restart()
        {
            tiles = Tiles.Restart();
            RefreshPage();
        }

        protected void RefreshPage()
        {
            this.StateHasChanged();
        }
    }
}