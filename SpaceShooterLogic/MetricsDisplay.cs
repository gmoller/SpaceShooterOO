using System.Collections.Generic;
using GuiControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class MetricsDisplay
    {
        private readonly Grid _grid;
 
        public MetricsDisplay()
        {
            GridColumn[] headers = {
                new GridColumn { HorizontalAlignment = HorizontalAlignment.Left, Text = "Name", X = 50.0f },
                new GridColumn { HorizontalAlignment = HorizontalAlignment.Right, Text = "Time (ms)", X = 245.0f },
                new GridColumn { HorizontalAlignment = HorizontalAlignment.Right, Text = "Frames", X = 335.0f },
                new GridColumn { HorizontalAlignment = HorizontalAlignment.Right, Text = "Avg. (ms)", X = 425.0f }
            };
            
            _grid = new Grid(AssetsManager.Instance.GetSpriteFont("arialSmall"), AssetsManager.Instance.GetSpriteFont("arialTiny"), Color.CornflowerBlue, new Vector2(50.0f, 400.0f), true, new GridColumns(headers));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _grid.ClearRows();
            foreach (KeyValuePair<string, Metric> entry in BenchmarkMetrics.Instance.Metrics)
            {
                var name = new GridColumn { Text = entry.Key, HorizontalAlignment = HorizontalAlignment.Left, X = 50.0f };
                var time = new GridColumn { Text = entry.Value._elapsedTime.ToString("F"), HorizontalAlignment = HorizontalAlignment.Right, X = 245.0f };
                var frames = new GridColumn { Text = entry.Value._frames.ToString(), HorizontalAlignment = HorizontalAlignment.Right, X = 335.0f };
                var avg = new GridColumn { Text = (entry.Value._elapsedTime / entry.Value._frames).ToString("F"), HorizontalAlignment = HorizontalAlignment.Right, X = 425.0f };

                var gridColumns = new GridColumns(name, time, frames, avg);
                var row = new GridRow(false, _grid, AssetsManager.Instance.GetSpriteFont("arialTiny"), Color.LightBlue, gridColumns);
                _grid.AddRow(row);
            }

            _grid.Draw(spriteBatch);
        }
    }
}