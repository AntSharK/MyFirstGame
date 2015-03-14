﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MyFirstGame.Screens;

namespace MyFirstGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class GameRunner : Game
    {
        /// <summary>
        /// Graphics device
        /// </summary>
        public GraphicsDeviceManager graphics;

        /// <summary>
        /// Spritebatch to draw sprites on
        /// </summary>
        public SpriteBatch spriteBatch;
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the keyboard state
            currentKeyboardState = Keyboard.GetState();

            // Update all active screens
            foreach (BaseScreen screen in screens)
            {
                if (screen.isActive)
                    screen.Update(gameTime);
            }
            base.Update(gameTime);
            previousKeyboardState = currentKeyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all visible screens.
            spriteBatch.Begin();
            foreach (BaseScreen screen in screens)
            {
                if (screen.isVisible)
                    screen.Draw(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
