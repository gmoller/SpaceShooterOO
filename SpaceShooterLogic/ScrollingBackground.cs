using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooterUtilities;

namespace SpaceShooterLogic
{
    public class ScrollingBackground
    {
        private readonly List<ScrollingBackgroundLayer> _layers = new List<ScrollingBackgroundLayer>();

        private readonly List<ScrollingBackgroundLayer> _layersDepth0 = new List<ScrollingBackgroundLayer>();
        private readonly List<ScrollingBackgroundLayer> _layersDepth1 = new List<ScrollingBackgroundLayer>();
        private readonly List<ScrollingBackgroundLayer> _layersDepth2 = new List<ScrollingBackgroundLayer>();

        public ScrollingBackground(List<string> textures)
        {
            for (int i = -1; i < 2; i++) // position indexes (-1,0,1)
            {
                // 3 layers
                for (int j = 0; j < 3; j++) // depths (0,1,2)
                {
                    int textureIndex = RandomGenerator.Instance.GetRandomInt(0, textures.Count - 1);
                    Texture2D texture = AssetsManager.Instance.GetTexture(textures[textureIndex]);
                    Vector2 position = new Vector2(0, texture.Height * i); // -640, 0, 640
                    Vector2 velocity = new Vector2(0, (j + 1) * 12.0f); // 12, 24, 36
                    ScrollingBackgroundLayer layer = new ScrollingBackgroundLayer(texture, j, i, position, velocity);
                    _layers.Add(layer);
                }
            }

            foreach (var layer in _layers)
            {
                switch (layer.Depth)
                {
                    case 0:
                    {
                        _layersDepth0.Add(layer);
                        break;
                    }
                    case 1:
                    {
                        _layersDepth1.Add(layer);
                        break;
                    }
                    case 2:
                    {
                        _layersDepth2.Add(layer);
                        break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(gameTime);
            }

            // Loop through layers in depth 0
            bool resetLayersDepth0 = false;
            foreach (var layer in _layersDepth0)
            {
                if (layer.PositionIndex == -1)
                {
                    if (layer.Position.Y > 0)
                    {
                        resetLayersDepth0 = true;
                    }
                }
            }

            // Loop through layers in depth 1
            bool resetLayersDepth1 = false;
            foreach (var layer in _layersDepth1)
            {
                if (layer.PositionIndex == -1)
                {
                    if (layer.Position.Y > 0)
                    {
                        resetLayersDepth1 = true;
                    }
                }
            }

            // Loop through layers in depth 2
            bool resetLayersDepth2 = false;
            foreach (var layer in _layersDepth2)
            {
                if (layer.PositionIndex == -1)
                {
                    if (layer.Position.Y > 0)
                    {
                        resetLayersDepth2 = true;
                    }
                }
            }

            if (resetLayersDepth0)
            {
                foreach (var layer in _layersDepth0)
                {
                    layer.Position = layer.InitialPosition;
                }
            }

            if (resetLayersDepth1)
            {
                foreach (var layer in _layersDepth1)
                {
                    layer.Position = layer.InitialPosition;
                }
            }

            if (resetLayersDepth2)
            {
                foreach (var layer in _layersDepth2)
                {
                    layer.Position = layer.InitialPosition;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in _layers)
            {
                layer.Draw(spriteBatch);
            }
        }
    }
}