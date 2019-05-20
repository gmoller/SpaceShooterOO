using System.Collections.Generic;
using GuiControls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic.Screens
{
    public class MetricsDisplay
    {
        private readonly Grid _grid;
 
        public MetricsDisplay()
        {
            var name = new GridColumn { Text = "Name", HorizontalAlignment = HorizontalAlignment.Left, X = 50.0f };
            var time = new GridColumn { Text = "Time (ms)", HorizontalAlignment = HorizontalAlignment.Right, X = 245.0f };
            var frames = new GridColumn { Text = "Frames", HorizontalAlignment = HorizontalAlignment.Right, X = 335.0f };
            var avg = new GridColumn { Text = "Avg. (ms)", HorizontalAlignment = HorizontalAlignment.Right, X = 425.0f };

            var gridColumns = new GridColumns(name, time, frames, avg);
            _grid = new Grid(AssetsManager.Instance.GetSpriteFont("arialSmall"), AssetsManager.Instance.GetSpriteFont("arialTiny"), Color.CornflowerBlue, new Vector2(50.0f, 400.0f), true, gridColumns);
        }

        public void Update(GameTime gameTime)
        {
            _grid.ClearRows();
            foreach (KeyValuePair<string, Metric> entry in BenchmarkMetrics.Instance.Metrics)
            {
                string name = entry.Key;
                string time = entry.Value._elapsedTime.ToString("F");
                string frames = entry.Value._frames.ToString();
                string avg = (entry.Value._elapsedTime / entry.Value._frames).ToString("F");

                var row = new GridRow(_grid, AssetsManager.Instance.GetSpriteFont("arialTiny"), Color.LightBlue, name, time, frames, avg);
                _grid.AddRow(row);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _grid.Draw(spriteBatch);
        }
    }
}