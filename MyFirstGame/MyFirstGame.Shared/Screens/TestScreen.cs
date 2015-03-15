using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites;
using MyFirstGame.Screens.ScreenDecorators;

namespace MyFirstGame.Screens
{
    public class TestScreen: BaseScreen
    {
        /// <summary>
        /// Internal class of strings, used for reserved keywords
        /// See debate on: enum vs const string
        /// </summary>
        public class Special
        {
            public const string Hero = "hero";
        }

        /// <summary>
        /// Initializes the test screen
        /// </summary>
        /// <param name="game">Our main game class</param>
        public TestScreen(GameRunner game): base(game)
        {
            //this.addSprite(new TestSprite(game), Special.Hero);
            this.addSprite(new TestAnimatedSprite(game), Special.Hero);
            this.addDecorator(new TestCameraDecorator(this));
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
		    base.Update(gameTime);

			if (InputState.IsDown(Keys.D))
            {
                this.reservedSprite[Special.Hero].position.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsDown(Keys.A))
            {
                this.reservedSprite[Special.Hero].position.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsDown(Keys.W))
            {
                this.reservedSprite[Special.Hero].position.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsDown(Keys.S))
            {
                this.reservedSprite[Special.Hero].position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
		
        }
    }
}
