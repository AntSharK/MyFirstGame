using System;
using System.Collections.Generic;
using MyFirstGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// The floor. Starts out simple, but should hold lots of logic later on.
    /// </summary>
    public class Floor : BaseSprite
    {
        /// <summary>
        /// Bottom of floor
        /// </summary>
        public float bottom;

        /// <summary>
        /// The floor below
        /// </summary>
        public Floor downstairs;

        /// <summary>
        /// The floor above
        /// </summary>
        public Floor upstairs;

        /// <summary>
        /// List of all shafts that are a part of this floor
        /// </summary>
        public List<Shaft> shafts = new List<Shaft>();

        /// <summary>
        /// Initializes a new floor. The bulk of the floor logic is handled in Building
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public Floor(Texture2D texture, Vector2 position) : base(texture, position)
		{
            this.bottom = this.GetBottom();
            this.downstairs = null;
            this.upstairs = null;
		}

        /// <summary>
        /// Sets the scale, and re-calculates the bottom
        /// </summary>
        /// <param name="newScale">Factor to scale by</param>
        public override void SetScale(float newScale)
        {
            base.SetScale(newScale);
            this.GetBottom();
        }

        /// <summary>
        /// Get the Y coordinate of the bottom
        /// </summary>
        /// <returns>Bottom as a Y coordinate</returns>
        private float GetBottom()
        {
			return this.position.Y + this.texture.Height;
        }
    }
}
