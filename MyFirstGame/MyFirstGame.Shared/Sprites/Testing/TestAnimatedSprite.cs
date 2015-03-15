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
            public const string Still = "default";
            public const string Accelerating = "accel";
            public const string Opening = "opening";
            public const string Opened = "opened";
            public const string Closing = "closing";
            public const string Deaccelerating = "decel";
            public const string Moving = "moving";
        }

        public string state;

        public float currentSpeed = 0;

        public float deacceleratePerSecond = 0.95f;

        public float maxSpeed = 2;

        public float cutOff = 0.2f;

        /// <summary>
        /// Initializes a new instance of the test animated sprite
        /// </summary>
        public TestAnimatedSprite():
            base(CurrentGame.content.Load<Texture2D>("Images\\testelevator.png"),
            //new Vector2(CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.X, CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + CurrentGame.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height / 2),
            new Vector2(200, 200),
            4, 2)
        {
            //this.addAnimation(0, 0, 7, 0, 0.1f, AnimationNames.Default, true);
            this.addAnimation(0, 0, 0, 0, 1f, AnimationNames.Still);
            this.addAnimation(0, 0, 3, 0, 0.2f, AnimationNames.Opening, false, false);
            this.addAnimation(3, 0, 0, 0, 0.2f, AnimationNames.Closing, false, false);
            this.addAnimation(3, 0, 3, 0, 1f, AnimationNames.Opened);
            this.addAnimation(0, 1, 3, 1, 0.15f, AnimationNames.Accelerating, false, false);
            this.addAnimation(3, 1, 0, 1, 0.15f, AnimationNames.Deaccelerating, false, false);
            this.addAnimation(3, 1, 3, 1, 0.15f, AnimationNames.Moving);
            this.currentAnimation = this.animations[AnimationNames.Still];

            this.state = AnimationNames.Still;
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
            if (InputState.IsKeyDown(Keys.X))
            {
                this.SetAnimation(AnimationNames.Opening);
            }
            if (InputState.IsKeyDown(Keys.C))
            {
                this.SetAnimation(AnimationNames.Closing);
            }
            if (InputState.IsKeyDown(Keys.V))
            {
                this.state = AnimationNames.Still;
                this.SetAnimation(AnimationNames.Still);
            }
            if (InputState.IsKeyDown(Keys.B))
            {
                this.state = AnimationNames.Opened;
                this.SetAnimation(AnimationNames.Opened);
            }
            if (InputState.IsKeyDown(Keys.N))
            {
                this.SetAnimation(AnimationNames.Accelerating);
            }
            if (InputState.IsKeyDown(Keys.M))
            {
                this.SetAnimation(AnimationNames.Deaccelerating);
            }
            if (InputState.IsKeyDown(Keys.OemComma))
            {
                this.SetAnimation(AnimationNames.Moving);
            }

            if (InputState.IsKeyDown(Keys.Up))
            {
                if (this.state == AnimationNames.Accelerating)
                {
                    this.currentSpeed = this.currentSpeed + (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.currentSpeed > this.maxSpeed)
                        this.currentSpeed = this.maxSpeed;
                }

                if (this.state == AnimationNames.Opened)
                {
                    this.state = AnimationNames.Closing;
                    this.SetAnimation(AnimationNames.Closing);
                }

                if (this.state == AnimationNames.Still)
                {
                    this.state = AnimationNames.Accelerating;
                    this.SetAnimation(AnimationNames.Accelerating);
                }
            }

            if (InputState.IsKeyDown(Keys.Down))
            {
                if (this.state == AnimationNames.Accelerating)
                {
                    this.currentSpeed = this.currentSpeed - (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.currentSpeed < -this.maxSpeed)
                        this.currentSpeed = -this.maxSpeed;
                }
                
                if (this.state == AnimationNames.Opened)
                {
                    this.state = AnimationNames.Closing;
                    this.SetAnimation(AnimationNames.Closing);
                }

                if (this.state == AnimationNames.Still)
                {
                    this.state = AnimationNames.Accelerating;
                    this.SetAnimation(AnimationNames.Accelerating);
                }
            }

			if (InputState.AreKeysUp(Keys.Up, Keys.Down))
            {
                this.currentSpeed = this.currentSpeed * this.deacceleratePerSecond;
                if ((this.currentSpeed < this.cutOff && this.currentSpeed > 0) || (this.currentSpeed > -this.cutOff && this.currentSpeed < 0))
                    this.currentSpeed = 0;

                if (this.state == AnimationNames.Accelerating || this.state == AnimationNames.Moving)
                {
                    this.SetAnimation(AnimationNames.Deaccelerating);
                    this.state = AnimationNames.Deaccelerating;
                }
                if (this.currentSpeed == 0 && this.state == AnimationNames.Deaccelerating && this.currentAnimation.isFinished)
                {
                    this.SetAnimation(AnimationNames.Opening);
                    this.state = AnimationNames.Opening;
                }
            }

            if (this.state == AnimationNames.Closing && this.currentAnimation.isFinished)
            {
                this.state = AnimationNames.Still;
                this.SetAnimation(AnimationNames.Still);
            }

            if (this.state == AnimationNames.Opening && this.currentAnimation.isFinished)
            {
                this.state = AnimationNames.Opened;
                this.SetAnimation(AnimationNames.Opened);
            }

            this.position.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100 * currentSpeed;
        }
    }
}
