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
        /// True if the elevator is on this floor
        /// </summary>
        private bool current;

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
        /// The people on this floor
        /// </summary>
        private MultiLayerSprite people;

        /// <summary>
        /// Gets or sets the people group
        /// </summary>
        public MultiLayerSprite People
        {
            get
            {
                return this.people;
            }
            set
            {
                this.people = value;
                this.screen.AddSprite(this.people);
            }
        }

        /// <summary>
        /// List of all shafts that are a part of this floor
        /// </summary>
        public List<ShaftSegment> shaft = new List<ShaftSegment>();

        /// <summary>
        ///  Gets or sets whether this floor is the current floor the elevator is at
        /// </summary>
        public bool Current
        {
            get
            {
                return this.current;
            }
            set
            {
                // If current, apply some fading. TEMPORARY MEASURE.
                if (value == true)
                {
                    this.fade = 0.5f;
                    foreach(ShaftSegment s in shaft)
                    {
                        s.fade = 0.8f;
                        People.isVisible = true;
                    }
                }
                // Otherwise, undo fading. TEMPORARY MEASURE.
                else
                {
                    this.fade = 1.0f;
                    foreach (ShaftSegment s in shaft)
                    {
                        s.fade = 1.0f;
                        People.isVisible = false;
                    }
                }
                this.current = value;
            }
        }

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
