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
            ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor1.png"));
            ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor2.png"));
            
            // TODO: Shaft gets its own sprite. This makes sense because when we add floors, shaft has to be elongated
			BaseAnimatedSprite shaftLv1 = new BaseAnimatedSprite (ContentLoader.GetTexture ("shaftbottom.png"), new Vector2 (0, 400), 5, 1);
			shaftLv1.addAnimation (0, 0, 4, 0, 0.25f, "animation");
			shaftLv1.SetAnimation ("animation");
            this.AddSprite(shaftLv1);
            BaseAnimatedSprite shaftLv2 = new BaseAnimatedSprite(ContentLoader.GetTexture("shaftmid.png"), new Vector2(0, 200), 5, 1);
            shaftLv2.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv2.SetAnimation("animation");
            this.AddSprite(shaftLv2);
            BaseAnimatedSprite shaftLv3 = new BaseAnimatedSprite(ContentLoader.GetTexture("shafttop.png"), new Vector2(0, 0), 5, 1);
            shaftLv3.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv3.SetAnimation("animation");
            this.AddSprite(shaftLv3);
            BaseAnimatedSprite shaftLv4 = new BaseAnimatedSprite(ContentLoader.GetTexture("shafttopflip.png"), new Vector2(0, -200), 5, 1);
            shaftLv4.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv4.SetAnimation("animation");
            this.AddSprite(shaftLv4);
            BaseAnimatedSprite shaftLv5 = new BaseAnimatedSprite(ContentLoader.GetTexture("shaftmidflip.png"), new Vector2(0, -400), 5, 1);
            shaftLv5.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv5.SetAnimation("animation");
            this.AddSprite(shaftLv5);
            BaseAnimatedSprite shaftLv6 = new BaseAnimatedSprite(ContentLoader.GetTexture("shaftmid.png"), new Vector2(0, -600), 5, 1);
            shaftLv6.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv6.SetAnimation("animation");
            this.AddSprite(shaftLv6);
            /*
            ((Building)this.reservedSprite[Elements.Tower]).addFloor(ContentLoader.GetTexture("floor3.png"));
            BaseAnimatedSprite shaftLv7 = new BaseAnimatedSprite(ContentLoader.GetTexture("shafttop.png"), new Vector2(0, -800), 5, 1);
            shaftLv7.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv7.SetAnimation("animation");
            this.AddSprite(shaftLv7);
            BaseAnimatedSprite shaftLv8 = new BaseAnimatedSprite(ContentLoader.GetTexture("shafttopflip.png"), new Vector2(0, -1000), 5, 1);
            shaftLv8.addAnimation(0, 0, 4, 0, 0.25f, "animation");
            shaftLv8.SetAnimation("animation");
            this.AddSprite(shaftLv8);
            */

            this.AddSprite(new Elevator(), Elements.Elevator);

			((Elevator)this.reservedSprite[Elements.Elevator]).CurrentBuilding = ((Building)this.reservedSprite[Elements.Tower]);

			this.camera = new Camera (800, 600) {
				Position = new Vector2 (400, 300)
			};
					
			CurrentGame.camera = this.camera;
            this.camera.SetTarget(this.reservedSprite[Elements.Elevator], "Y");
            this.AddDecorator(new TestCameraDecorator(this));
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);
			//camera.ClampToArea (0, 0, 800, 600);
			
            /*
            Elevator elevator = (Elevator)this.reservedSprite [Elements.Elevator];
            
			if (elevator.position.Y < 0) {
				elevator.position.Y = 0;
				camera.shake (0.7f, 25);
			}
			if (elevator.position.Y + elevator.texture.Height / elevator.numberOfRows > CurrentGame.graphics.GraphicsDevice.Viewport.Height) {
				elevator.position.Y = CurrentGame.graphics.GraphicsDevice.Viewport.Height - elevator.texture.Height / elevator.numberOfRows;
				camera.shake (0.7f, 25);
			}
            */
        }
    }
}
