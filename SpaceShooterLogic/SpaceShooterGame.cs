using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using SpaceShooterLogic.GameStates;
using SpaceShooterUtilities;
using SpriteFontPlus;

namespace SpaceShooterLogic
{
    public class SpaceShooterGame
    {
        private FramesPerSecondCounter _fps;
        private SpriteFont _font;
        private ScrollingBackground _scrollingBackground;

        private IGameState _gameState;

        private Label _lblTest;
        private Label _lblFps;

        private readonly Stopwatch _updateStopwatch = new Stopwatch();
        private readonly Stopwatch _drawStopwatch = new Stopwatch();
        private int _updateFrames;
        private int _drawFrames;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            string[] ttfFiles = GetAnyFilesFromContentDirectory("*.ttf");
            List<SpriteFont> fonts = BakeTtfFiles(ttfFiles, graphicsDevice);
            AddToAssetsManager(ttfFiles, fonts);

            string[] pngFiles = GetAnyFilesFromContentDirectory("*.png");
            List<Texture2D> textures = BakeTextures(pngFiles, graphicsDevice);
            AddToAssetsManager(pngFiles, textures);
        }

        private string[] GetAnyFilesFromContentDirectory(string searchPattern)
        {
            string path = $@"{Directory.GetCurrentDirectory()}\Content\";
            var directoryInfo = new DirectoryInfo(path);

            FileInfo[] ttfFiles = { };
            if (directoryInfo.Exists)
            {
                // get any ttf files
                ttfFiles = directoryInfo.GetFiles(searchPattern);
            }

            return ttfFiles.Select(item => item.FullName).ToArray();
        }

        private List<SpriteFont> BakeTtfFiles(string[] ttfFiles, GraphicsDevice graphicsDevice)
        {
            var fonts = new List<SpriteFont>();

            foreach (string file in ttfFiles)
            {
                TtfFontBakerResult bakeResult = TtfFontBaker.Bake(File.ReadAllBytes(file), 50, 256, 256, new[]
                {
                    CharacterRange.BasicLatin,
                    //CharacterRange.Latin1Supplement,
                    //CharacterRange.LatinExtendedA,
                    //CharacterRange.LatinExtendedB,
                    //CharacterRange.Cyrillic,
                    //CharacterRange.CyrillicSupplement,
                    //CharacterRange.Hiragana,
                    //CharacterRange.Katakana
                });
                SpriteFont font = bakeResult.CreateSpriteFont(graphicsDevice);
                fonts.Add(font);
            }

            return fonts;
        }

        private List<Texture2D> BakeTextures(string[] pngFiles, GraphicsDevice graphicsDevice)
        {
            var textures = new List<Texture2D>();

            foreach (string file in pngFiles)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    textures.Add(texture);
                }
            }

            return textures;
        }

        private void AddToAssetsManager(string[] files, List<SpriteFont> fonts)
        {
            int i = 0;
            foreach (SpriteFont font in fonts)
            {
                AssetsManager.Instance.AddSpriteFont(Path.GetFileNameWithoutExtension(files[i++]), font);
            }
        }

        private void AddToAssetsManager(string[] files, List<Texture2D> textures)
        {
            int i = 0;
            foreach (Texture2D texture in textures)
            {
                AssetsManager.Instance.AddTexture(Path.GetFileNameWithoutExtension(files[i++]), texture);
            }
        }

        public void LoadContent(ContentManager content, int width, int height)
        {
            AssetsManager.Instance.ContentManager = content;
            DeviceManager.Instance.Viewport = new Rectangle(0, 0, width, height);

            // Load textures
            AssetsManager.Instance.AddTextures("sprBg0", "sprBg1", "sprBtnPlay", "sprBtnPlayDown", "sprBtnPlayHover", "sprBtnRestart", "sprBtnRestartDown", "sprBtnRestartHover", "sprPlayer", "sprLaserPlayer", "sprLaserEnemy0", "sprExplosion", "sprEnemy0", "sprEnemy1", "sprEnemy2", "Explosion50", "Fireball02");
            // Load sounds
            AssetsManager.Instance.AddSounds("sndBtnDown", "sndBtnOver", "sndLaser", "sndExplode0", "sndExplode1");
            // Load sprite fonts
            AssetsManager.Instance.AddSpriteFonts("arialHeading", "arialSmall", "arialTiny");
            _font = AssetsManager.Instance.GetSpriteFont("arialTiny");

            _scrollingBackground = new ScrollingBackground(new List<string> { "sprBg0", "sprBg1" });

            _gameState = new MainMenuState();
            _gameState.Enter();

            _fps = new FramesPerSecondCounter();

            _lblTest = new Label(_font, VerticalAlignment.Top, HorizontalAlignment.Left, new Vector2(0.0f, 0.0f), "Testing, testing, testing, 1, 2, 3...", Color.Cyan, 1.0f, 0.5f);
            _lblFps = new Label(_font, VerticalAlignment.Bottom, HorizontalAlignment.Right, DeviceManager.Instance.ScreenDimensions, "FPS: ", Color.Cyan) { TextShadow = true };
        }

        public void Update(GameTime gameTime)
        {
            _updateFrames++;
            _updateStopwatch.Start();

            _fps.Update(gameTime);
            _scrollingBackground.Update(gameTime);

            (bool changeGameState, IGameState newGameState) returnGameState = _gameState.Update(gameTime);
            if (returnGameState.changeGameState) ChangeGameState(returnGameState.newGameState);

            _updateStopwatch.Stop();
            BenchmarkMetrics.Instance.Metrics["SpaceShooterGame.Update"] = new Metric(_updateStopwatch.Elapsed.TotalMilliseconds, _updateFrames);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _drawFrames++;
            _drawStopwatch.Start();

            _fps.Draw();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            _scrollingBackground.Draw(spriteBatch);
            _gameState.Draw(spriteBatch);

            _lblTest.Draw(spriteBatch);
            _lblFps.Text = $"FPS: {_fps.FramesPerSecond}";
            _lblFps.Draw(spriteBatch);

            spriteBatch.End();

            _drawStopwatch.Stop();
            BenchmarkMetrics.Instance.Metrics["SpaceShooterGame.Draw"] = new Metric(_drawStopwatch.Elapsed.TotalMilliseconds, _drawFrames);
        }

        private void ChangeGameState(IGameState newGameState)
        {
            _gameState.Leave();
            _gameState = newGameState;
            _gameState.Enter();
        }
    }
}