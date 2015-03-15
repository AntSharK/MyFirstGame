using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Text;

namespace MyFirstGame
{
    public static class CurrentGame
    {
        /// <summary>
        /// Reference to current game
        /// </summary>
        public static GameRunner game;

        /// <summary>
        /// Graphics device
        /// </summary>
        public static GraphicsDeviceManager graphics;

        /// <summary>
        /// Spritebatch to draw sprites on
        /// </summary>
        public static SpriteBatch spriteBatch;

        /// <summary>
        /// Content manager of game
        /// </summary>
        public static ContentManager content;
    }
}
