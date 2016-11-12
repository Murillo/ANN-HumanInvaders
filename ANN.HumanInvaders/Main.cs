﻿using MachineLearning.Technics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ANN.HumanInvaders
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MultilayerPerceptron mlp;
        Texture2D image1, image2;
        Vector2 vector1, vector2;
        float speed = 4;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            image1 = Content.Load<Texture2D>("database");
            image2 = Content.Load<Texture2D>("hospital");

            Random r = new Random();
            vector1 = new Vector2(10, 240);
            vector2 = new Vector2(r.Next(400, 600), r.Next(0, 400));

            // Input = Position.X Player, Position.Y Player, Position.X Enemy, Position.Y Enemy
            // Output = Direction, Speed
            mlp = new MultilayerPerceptron(4, 5, 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                vector1.Y -= speed;
                double[,] positions = new double[,] { { vector1.X, vector1.Y, vector2.X, vector2.Y } };
                double[] result = new double[] { 0 };
                mlp.Training(positions, result);
                System.Diagnostics.Debug.WriteLine(mlp.Run(new double[] { vector1.X, vector1.Y, vector2.X, vector2.Y })[0]);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                vector1.Y += speed;
                double[,] positions = new double[,] { { vector1.X, vector1.Y, vector2.X, vector2.Y } };
                double[] result = new double[] { 1 };
                mlp.Training(positions, result);
                System.Diagnostics.Debug.WriteLine(mlp.Run(new double[] { vector1.X, vector1.Y, vector2.X, vector2.Y })[0]);
            }
            else
            {
                double[,] positions = new double[,] { { vector1.X, vector1.Y, vector2.X, vector2.Y } };
                double[] result = new double[] { 0.5 };
                mlp.Training(positions, result);
                System.Diagnostics.Debug.WriteLine(mlp.Run(new double[] { vector1.X, vector1.Y, vector2.X, vector2.Y })[0]);
            }

            if (vector2.X >= 0)
                vector2.X -= 2;
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(image1, vector1, new Rectangle(0,0,50,50), Color.White);
            spriteBatch.Draw(image2, vector2, new Rectangle(0, 0, 50, 50), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}