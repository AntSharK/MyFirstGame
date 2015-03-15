using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Screens;

namespace MyFirstGame.Sprites
{
    public class BaseSprite
    {
        /// <summary>
        /// Texture that this sprite displays
        /// </summary>
        public Texture2D texture;
        
        /// <summary>
        /// Position of sprite
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// Whether sprite is active. Active sprites are updated.
        /// </summary>
        public bool isActive;

        /// <summary>
        /// Whether sprite is visible. Visible sprites are drawn.
        /// </summary>
        public bool isVisible;

        /// <summary>
        /// Screen this sprite is attached to. Sprites cannot exist without screens/stages.
        /// </summary>
        public BaseScreen screen;

        /// <summary>
        /// Initializes the sprite
        /// </summary>
        /// <param name="texture">Enter a texture here to load</param>
        /// <param name="position">Enter a position here</param>
        public BaseSprite(Texture2D texture, Vector2 position)
        {
            this.initialize(texture, position);
        }

        /// <summary>
        /// Initializes a new sprite
        /// </summary>
        /// <param name="texture">Texture to draw for the sprite</param>
        /// <param name="position">Position, as a vector2</param>
        /// <param name="screen">Screen to attach the sprite to</param>
        public void initialize(Texture2D texture, Vector2 position, BaseScreen screen = null)
        {
            this.texture = texture;
            this.position = position;
            this.isActive = true;
            this.isVisible = true;
            this.screen = screen;
        }

        /// <summary>
        /// Draws the sprite for the spritebatch
        /// </summary>
        /// <param name="batch">Spritebatch to draw on</param>
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(this.texture, this.position, Color.White);
        }

        /// <summary>
        /// Draws the sprite for the current game's spritebatch
        /// </summary>
        public virtual void Draw()
        {
            CurrentGame.spriteBatch.Draw(this.texture, this.position, Color.White);
        }

        /// <summary>
        /// Override this method
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public virtual void Update(GameTime gameTime) { }
    }
}
