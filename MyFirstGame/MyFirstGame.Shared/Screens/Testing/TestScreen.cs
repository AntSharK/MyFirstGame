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
        public class Elements
        {
            public const string Elevator = "elevator";
            public const string Tower = "tower";
            public const string Shaft = "shaft";
        }

        /// <summary>
        /// Initializes the test screen
        /// </summary>
        /// <param name="game">Our main game class</param>
        public TestScreen(): base()
        {
            this.AddSprite(new Building(), Elements.Tower);
            ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor1.png"));
           ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor2.png"));
           ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor3.png"));

			BaseAnimatedSprite shaft = new BaseAnimatedSprite (ContentLoader.GetTexture ("shaft.png"), new Vector2 (0, 0), 5, 1);
			shaft.addAnimation (0, 0, 4, 0, 0.25f, "animation");
			shaft.SetAnimation ("animation");
            this.AddSprite(shaft);
            this.AddSprite(new Elevator(), Elements.Elevator);

			((Elevator)this.reservedSprite[Elements.Elevator]).currentBuilding = ((Building)this.reservedSprite[Elements.Tower]);

			this.camera = new Camera (800, 600) {
				Position = new Vector2 (400, 300)
			};

			CurrentGame.camera = camera;
			this.camera.spriteToFollow = ((Elevator)this.reservedSprite [Elements.Elevator]);

          //  TestCameraDecorator camera = new TestCameraDecorator(this);
         //   camera.AttachToSprite(this.reservedSprite[Elements.Elevator]);
         //   this.AddDecorator(camera);

        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
			
		    base.Update(gameTime);
			camera.ClampToArea (0, 0, 800, 600);
			Elevator elevator = (Elevator)this.reservedSprite [Elements.Elevator];
            
			if (elevator.position.Y < 0) {
				elevator.position.Y = 0;
				camera.shake (0.7f, 25);
			}
			if (elevator.position.Y + elevator.texture.Height / elevator.numberOfRows > CurrentGame.graphics.GraphicsDevice.Viewport.Height) {
				elevator.position.Y = CurrentGame.graphics.GraphicsDevice.Viewport.Height - elevator.texture.Height / elevator.numberOfRows;
				camera.shake (0.7f, 25);
			}
            
        }
    }
}
