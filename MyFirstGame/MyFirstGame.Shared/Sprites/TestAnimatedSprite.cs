using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MyFirstGame.Sprites;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Initializes a test animated sprite
    /// </summary>
    public class TestAnimatedSprite : BaseAnimatedSprite
    {
        /// <summary>
        /// Animation names
        /// </summary>
        public class AnimationNames
        {
            public const string Default = "default";
            public const string Default2 = "default2";
        }

        /// <summary>
        /// Initializes a new instance of the test animated sprite
        /// </summary>
        public TestAnimatedSprite():
            base(CurrentGame.content.Load<Texture2D>("Images\\turretplatformspawntransclucent.png"),
            new Vector2(CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.X, CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height / 2),
            8, 1)
        {
            this.addAnimation(0, 0, 7, 0, 0.1f, AnimationNames.Default, true);
            this.addAnimation(4, 0, 7, 0, 0.1f, AnimationNames.Default2);
            this.currentAnimation = this.animations[AnimationNames.Default];
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputState.IsKeyDown(Keys.Z))
            {
                this.isVisible = !this.isVisible;
            }
        }
    }
}
