using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MyFirstGame.Sprites;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Initializes a test animated sprite
    /// </summary>
    public class TestAnimatedSprite : BaseAnimatedSprite
    {
        public class AnimationNames
        {
            public const string Default = "default";
            public const string Default2 = "default2";
        }

        public TestAnimatedSprite(GameRunner game):
            base(game.Content.Load<Texture2D>("Images\\turretplatformspawntransclucent.png"),
            new Vector2(game.GraphicsDevice.Viewport.TitleSafeArea.X, game.GraphicsDevice.Viewport.TitleSafeArea.Y + game.GraphicsDevice.Viewport.TitleSafeArea.Height / 2),
            8, 1)
        {
            this.addAnimation(0, 0, 7, 0, 0.1f, AnimationNames.Default, true);
            //this.addAnimation(4, 0, 7, 1, 0.1f, AnimationNames.Default2);
            this.currentAnimation = this.animations[AnimationNames.Default];
        }
    }
}
