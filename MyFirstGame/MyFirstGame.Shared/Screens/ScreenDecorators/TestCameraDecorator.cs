using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites;

namespace MyFirstGame.Screens.ScreenDecorators
{
    /// <summary>
    /// The camera is a decorator that acts on all the elements on a screen to transform them
    /// </summary>
    public class TestCameraDecorator : BaseScreenDecorator
    {
        /// <summary>
        /// Sprite the camera is attached to
        /// </summary>
        public BaseSprite attachedSprite;
        
        /// <summary>
        /// Old position of the sprite this camera is attached to
        /// </summary>
        public Vector2 oldAttachmentPosition;

        /// <summary>
        /// Attaches a test camera to a screen. Cameras trasform every sprite on the screen.
        /// </summary>
        /// <param name="screen">Screen to attach to</param>
        public TestCameraDecorator(BaseScreen screen) : base(screen) { }

        /// <summary>
        /// Acts on screen, called once every cycle
        /// </summary>
        /// <param name="gameTime">Game Time to calculate elapsed time</param>
        public override void Act(GameTime gameTime)
        {
            if (InputState.IsKeyDown(Keys.W))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.Y = s.position.Y +  CurrentGame.getDelta(gameTime) * 100;
                }
            }
            if (InputState.IsKeyDown(Keys.S))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.Y = s.position.Y - CurrentGame.getDelta(gameTime) * 100;
                }
            }
            if (InputState.IsKeyDown(Keys.A))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.X = s.position.X + CurrentGame.getDelta(gameTime) * 100;
                }
            }
            if (InputState.IsKeyDown(Keys.D))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
					s.position.X = s.position.X - CurrentGame.getDelta(gameTime) * 100;
                }
            }
            if (InputState.IsKeyDown(Keys.E))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.setScale(1 + CurrentGame.getDelta(gameTime));
                }
            }
            if (InputState.IsKeyDown(Keys.Q))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.setScale(1 - CurrentGame.getDelta(gameTime));
                }
            }

            if (InputState.IsKeyDown(Keys.R))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.rotation = s.rotation + CurrentGame.getDelta(gameTime);
                }
            }

            /*
            if (this.attachedSprite != null)
            {
                if (this.attachedSprite.position != this.oldAttachmentPosition)
                {
                    Vector2 delta = this.attachedSprite.position - this.oldAttachmentPosition;
                    foreach (BaseSprite s in this.screen.sprites)
                    {
                        s.position = s.position - delta/100;
                    }
                }
            }
            */
        }

        /// <summary>
        /// Attaches camera to a sprite
        /// </summary>
        /// <param name="sprite">Sprite to attach to</param>
        public void AttachToSprite(BaseSprite sprite)
        {
            this.attachedSprite = sprite;
            this.oldAttachmentPosition = sprite.position;
        }
    }
}
