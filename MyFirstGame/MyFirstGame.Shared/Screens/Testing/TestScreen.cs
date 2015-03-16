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


		Elevator elevator;
        /// <summary>
        /// Initializes the test screen
        /// </summary>
        /// <param name="game">Our main game class</param>
        public TestScreen(): base()
        {
			elevator = new Elevator ();
			Building building = new Building ();
            //this.addSprite(new TestSprite(game), Special.Hero);
            this.addSprite(elevator, Special.Hero);
			this.addSprite (building);
			elevator.currentBuilding = building;
            //this.addDecorator(new TestCameraDecorator(this));
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
		    base.Update(gameTime);

			if (InputState.IsKeyDown(Keys.D))
            {
                this.reservedSprite[Special.Hero].position.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsKeyDown(Keys.A))
            {
                this.reservedSprite[Special.Hero].position.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsKeyDown(Keys.W))
            {
                this.reservedSprite[Special.Hero].position.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }
			if (InputState.IsKeyDown(Keys.S))
            {
                this.reservedSprite[Special.Hero].position.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
            }


			if (elevator.position.Y < 0) {
				elevator.position.Y = 0;
			}
			if (elevator.position.Y + elevator.texture.Height / elevator.numberOfRows > CurrentGame.graphics.GraphicsDevice.Viewport.Height) {
				elevator.position.Y = CurrentGame.graphics.GraphicsDevice.Viewport.Height - elevator.texture.Height / elevator.numberOfRows;
			}
        }
    }
}
