using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Base class for an animated sprite
    /// </summary>
    public class BaseAnimatedSprite : BaseSprite
    {
        /// <summary>
        /// Number of columns to chop the texture into
        /// </summary>
        public int numberOfColumns;
        
        /// <summary>
        /// Number of rows to chop the texture into
        /// </summary>
        public int numberOfRows;

        /// <summary>
        /// Animations and their names
        /// </summary>
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        /// <summary>
        /// Currently active animation
        /// </summary>
        public Animation currentAnimation;

        /// <summary>
        /// Initializes a new animated sprite
        /// </summary>
        /// <param name="texture">Texture to put into the sprite</param>
        /// <param name="position">Position of sprite</param>
        /// <param name="numberOfColumns">Number of columns to chop texture into</param>
        /// <param name="numberOfRows">Number of rows to chop texture into</param>
        public BaseAnimatedSprite(Texture2D texture, Vector2 position, int numberOfColumns = 1, int numberOfRows = 1): base(texture, position)
        {
            this.numberOfColumns = numberOfColumns;
            this.numberOfRows = numberOfRows;
        }

        /// <summary>
        /// To be overridden. Just updates the current animation if it is active.
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.currentAnimation.isActive)
                this.currentAnimation.Update(gameTime);
        }

        /// <summary>
        /// Draws the current animation. Don't need to check if it's visible, since visibility is a sprite issue.
        /// </summary>
        /// <param name="batch">Spritebatch to draw on</param>
        public override void Draw(SpriteBatch batch)
        {
            this.currentAnimation.Draw(batch);
        }

        /// <summary>
        /// Adds an animation to the animated sprite
        /// </summary>
        /// <param name="xStart">X position of square to start from, with left being 0</param>
        /// <param name="yStart">Y position of square to start from, with top being 0.</param>
        /// <param name="xEnd">X position of square to end on. If we have 8 columns, the rightmost square is 7.</param>
        /// <param name="yEnd">Y position of square to end on. If we have 7 rows, the bottommost square is 6.</param>
        /// <param name="timePerFrame">Time per frame</param>
        /// <param name="animationName">Name to give the animation</param>
        /// <returns></returns>
        public bool addAnimation(int xStart, int yStart, int xEnd, int yEnd, float timePerFrame = 0.1f, string animationName = "", bool isReversible = false)
        {
            // Return false if there are invalid parameters
            if (xStart >= this.numberOfColumns || xEnd >= this.numberOfColumns || yEnd >= this.numberOfRows || yEnd < yStart)
            {
                return false;
            }
            // Otherwise try creating a new animation and adding it
            try
            {
                Animation newAnimation = new Animation(this, xStart, yStart, xEnd, yEnd, timePerFrame, isReversible);
                this.animations.Add(animationName, newAnimation);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Stores an animation
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Whether the animation is active. Inactive animations can be drawn on active sprites, but don't animate.
        /// </summary>
        public bool isActive;

        /// <summary>
        /// Current frame number
        /// </summary>
        public int currentFrame;

        /// <summary>
        /// Total number of frames
        /// </summary>
        public int numberOfFrames;

        /// <summary>
        /// Time between each frame
        /// </summary>
        public float timePerFrame;

        /// <summary>
        /// Time since last frame change
        /// </summary>
        public float timeElapsed;

        /// <summary>
        /// The sprite this animation is attached to
        /// </summary>
        public BaseAnimatedSprite sprite;

        /// <summary>
        /// The x position of the square of the texture this animation starts at
        /// </summary>
        public int xStart;

        /// <summary>
        /// The y position of the square of the texture this animation starts at
        /// </summary>
        public int yStart;

        /// <summary>
        /// The x position of the ending square of the texture for this animation
        /// </summary>
        public int xEnd;

        /// <summary>
        /// The y position of the ending square of the texture for this animation
        /// </summary>
        public int yEnd;

        /// <summary>
        /// The width of a square this animation takes up
        /// </summary>
        public int frameWidth;

        /// <summary>
        /// The height of a square that this animation takes up
        /// </summary>
        public int frameHeight;

        /// <summary>
        /// The rectangle array to mark out where in the texture this animation's frames are
        /// </summary>
        public Rectangle[] rectangles;

        /// <summary>
        /// Reversible animations go back and forth
        /// </summary>
        public bool isReversible;

        /// <summary>
        /// Initializes a new animation
        /// </summary>
        /// <param name="sprite">Sprite to attach animation to</param>
        /// <param name="xStart">X position of square in sprite to start this animation</param>
        /// <param name="yStart">Y position of square in sprite to start this animation</param>
        /// <param name="xEnd">X position of square in sprite to end this animation</param>
        /// <param name="yEnd">Y position of square in sprite to end this animation</param>
        /// <param name="timePerFrame">Duration of each frame</param>
        public Animation(BaseAnimatedSprite sprite, int xStart, int yStart, int xEnd, int yEnd, float timePerFrame = 0, bool isReversible = false)
        {
            // Initialize a bunch of stuff
            this.isActive = true;
            this.currentFrame = 0;
            this.timePerFrame = timePerFrame;
            this.timeElapsed = 0;
            this.sprite = sprite;
            this.xStart = xStart;
            this.yStart = yStart;
            this.xEnd = xEnd;
            this.yEnd = yEnd;
            this.numberOfFrames = sprite.numberOfColumns-xStart + sprite.numberOfColumns * (yEnd - yStart - 1) + xEnd + 1;
            this.isReversible = isReversible;
            this.frameWidth = sprite.texture.Width / sprite.numberOfColumns;
            this.frameHeight = sprite.texture.Height / sprite.numberOfRows;

            // Initialize rectangle positions
            if (this.isReversible && numberOfFrames > 1)
            {
                rectangles = new Rectangle[numberOfFrames * 2 - 2];
            }
            else
            {
                rectangles = new Rectangle[numberOfFrames];
            }

            int x = xStart;
            int y = yStart;
            for (int i = 0; i < numberOfFrames; i++)
            {
                rectangles[i] = new Rectangle(x * this.frameWidth, y * this.frameHeight, this.frameWidth, this.frameHeight);
                x++;
                if (x >= this.sprite.numberOfColumns)
                {
                    x = 0;
                    y = y + 1;
                }
            }

            // Reversible animations have extra rectangles!
            if (this.isReversible && numberOfFrames > 1)
            {
                for (int i = 0; i < numberOfFrames - 2; i++)
                {
                    rectangles[numberOfFrames + i] = rectangles[numberOfFrames - i - 2];
                }

                this.numberOfFrames = this.numberOfFrames * 2 - 2;
            }
        }

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.isActive)
            {
                timeElapsed = timeElapsed + (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (timeElapsed > timePerFrame)
                {
                    currentFrame++;
                    timeElapsed = timeElapsed - timePerFrame;
                }
                while (currentFrame >= numberOfFrames)
                {
                    currentFrame = currentFrame - numberOfFrames;
                }
            }
        }

        /// <summary>
        /// Draws the current frame of this animation
        /// </summary>
        /// <param name="batch">Sprite batch to draw on</param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.sprite.texture, this.sprite.position, this.rectangles[this.currentFrame], Color.White);
        }

        /// <summary>
        /// Resets current animation
        /// </summary>
        public void Reset()
        {
            this.currentFrame = 0;
            this.timeElapsed = 0;
        }

        /// <summary>
        /// Stops current animation
        /// </summary>
        public void Stop()
        {
            this.isActive = false;
            this.Reset();
        }
    }
}
