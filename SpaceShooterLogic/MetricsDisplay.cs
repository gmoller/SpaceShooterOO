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
            ColumnHeaders headers = new ColumnHeaders(
                new ColumnHeader {Name = "Name", HorizontalAlignment = HorizontalAlignment.Left, X = 50.0f},
                new ColumnHeader {Name = "Time (ms)", HorizontalAlignment = HorizontalAlignment.Right, X = 245.0f},
                new ColumnHeader {Name = "Frames", HorizontalAlignment = HorizontalAlignment.Right, X = 335.0f},
                new ColumnHeader {Name = "Avg. (ms)", HorizontalAlignment = HorizontalAlignment.Right, X = 425.0f});

            _grid = new Grid(AssetsManager.Instance.GetSpriteFont("arialSmall"), AssetsManager.Instance.GetSpriteFont("arialTiny"), Color.CornflowerBlue, new Vector2(50.0f, 400.0f), true, headers);
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

                var row = new GridRow(_grid, Color.LightBlue, name, time, frames, avg);
                _grid.AddRow(row);
            }

            _grid.Draw(spriteBatch);

            //float y = 425.0f;
            //foreach (KeyValuePair<string, Metric> entry in BenchmarkMetrics.Instance.Metrics)
            //{
            //    // Name
            //    float x = 50.0f;
            //    var lbl1 = new Label(_font, VerticalAlignment.Top, HorizontalAlignment.Left, new Vector2(x, y), entry.Key, Color.LightBlue);
            //    lbl1.Draw(spriteBatch);

            //    // Time
            //    x = 245.0f;
            //    var lbl2 = new Label(_font, VerticalAlignment.Top, HorizontalAlignment.Right, new Vector2(x, y), entry.Value._elapsedTime.ToString("F"), Color.LightBlue);
            //    lbl2.Draw(spriteBatch);

            //    x = 335.0f;
            //    var lbl3 = new Label(_font, VerticalAlignment.Top, HorizontalAlignment.Right, new Vector2(x, y), entry.Value._frames.ToString(), Color.LightBlue);
            //    lbl3.Draw(spriteBatch);

            //    double avg = entry.Value._elapsedTime / entry.Value._frames;
            //    x = 425.0f;
            //    var lbl4 = new Label(_font, VerticalAlignment.Top, HorizontalAlignment.Right, new Vector2(x, y), avg.ToString("F"), Color.LightBlue);
            //    lbl4.Draw(spriteBatch);

            //    y += 15.0f;
            //}
        }
    }
}