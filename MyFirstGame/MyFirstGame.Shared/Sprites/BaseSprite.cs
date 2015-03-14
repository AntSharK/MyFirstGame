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
        public Texture2D texture;
        public Vector2 position;
        public bool isActive;
        public bool isVisible;
        public BaseScreen screen;

        public BaseSprite(Texture2D texture, Vector2 position)
        {
            this.initialize(texture, position);
        }

        public void initialize(Texture2D texture, Vector2 position, BaseScreen screen = null)
        {
            this.texture = texture;
            this.position = position;
            this.isActive = true;
            this.isVisible = true;
            this.screen = screen;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.texture, this.position, Color.White);
        }

        /// <summary>
        /// Override this method
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public virtual void Update(GameTime gameTime) { }
    }
}
