using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites;

namespace MyFirstGame.Screens.ScreenDecorators
{
    public class TestCameraDecorator : BaseScreenDecorator
    {
        public TestCameraDecorator(BaseScreen screen) : base(screen) { }

        public override void Act(GameTime gameTime)
        {
            if (InputState.IsDown(Keys.Down))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.Y = s.position.Y + (float) gameTime.ElapsedGameTime.TotalSeconds * 100;
                }
            }
            if (InputState.IsDown(Keys.Up))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.Y = s.position.Y - (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                }
            }
            if (InputState.IsDown(Keys.Left))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.X = s.position.X + (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                }
            }
            if (InputState.IsDown(Keys.Right))
            {
                foreach (BaseSprite s in this.screen.sprites)
                {
                    s.position.X = s.position.X - (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                }
            }
        }
    }
}
