﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooterLogic;
using SpaceShooterUtilities;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IGame _game;

        private readonly Stopwatch _updateStopwatch = new Stopwatch();
        private readonly Stopwatch _drawStopwatch = new Stopwatch();
        private int _updateFrames;
        private int _drawFrames;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Position = Point.Zero;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 480; // 480
            _graphics.PreferredBackBufferHeight = 640; // 640
            _graphics.SynchronizeWithVerticalRetrace = false;

            VariableTimeStep();
            //FixedTimeStep();

            _graphics.ApplyChanges();

            _game = new SpaceShooterGame();
            //_game = new TestBedGame();
            _game.Initialize(Window, GraphicsDevice);

            base.Initialize();
        }

        private void VariableTimeStep()
        {
            IsFixedTimeStep = false;
        }

        private void FixedTimeStep()
        {
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60); // 60fps
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _game.LoadContent(Content, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _updateFrames++;
            _updateStopwatch.Start();

            IsMouseVisible = DeviceManager.Instance.IsMouseVisible;

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape) && keyboardState.IsKeyDown(Keys.LeftShift))
            {
                Exit();
            }

            // TODO: Add your update logic here
            _game.Update(gameTime);

            base.Update(gameTime);

            _updateStopwatch.Stop();
            BenchmarkMetrics.Instance.Metrics["Overall.Update"] = new Metric(_updateStopwatch.Elapsed.TotalMilliseconds, _updateFrames);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _drawFrames++;
            _drawStopwatch.Start();

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _game.Draw(_spriteBatch);

            base.Draw(gameTime);

            _drawStopwatch.Stop();
            BenchmarkMetrics.Instance.Metrics["Overall.Draw"] = new Metric(_drawStopwatch.Elapsed.TotalMilliseconds, _drawFrames);
        }
    }
}