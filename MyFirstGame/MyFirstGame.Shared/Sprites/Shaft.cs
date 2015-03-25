using System;
using System.Collections.Generic;
using MyFirstGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// The elevator shaft. Should be pretty simple. Most of the logic is in Building.
    /// </summary>
    public class Shaft : BaseAnimatedSprite
    {
        /// <summary>
        /// Bottom of this section of the shaft
        /// </summary>
        public float bottom;

        /// <summary>
        /// The shaft below
        /// </summary>
        public Shaft below;

        /// <summary>
        /// The shaft above
        /// </summary>
        public Shaft above;

        /// <summary>
        /// The floor that this shaft is on
        /// </summary>
        public Floor floor;

        /// <summary>
        /// Initializes a new floor. The bulk of the floor logic is handled in Building
        /// </summary>
        /// <param name="texture">Texture to use</param>
        /// <param name="position">Position</param>
        /// <param name="numberOfColumns">Number of Columns</param>
        /// <param name="numberOfRows">Number of rows</param>
        public Shaft(Texture2D texture, Vector2 position, int numberOfColumns = 1, int numberOfRows = 1)
            : base(texture, position, numberOfColumns, numberOfRows)
        {
            this.bottom = this.GetBottom();
            this.above = null;
            this.below = null;
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
        /// Given a list of floors, bind to the best one
        /// The floor whose base is below or equal to the shaft's base
        /// </summary>
        /// <param name="floors">List of candidate floors</param>
        /// <returns>True if binded, false otherwise</returns>
        public bool BindToFloor(List<Floor> floors)
        {
            foreach (Floor floor in floors)
            {
                if (floor.bottom >= this.bottom && floor.bottom - floor.texture.Height <= this.bottom)
                {
                    BindToFloor(floor);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Binds to the floor
        /// </summary>
        /// <param name="floor">The floor to bind to</param>
        public void BindToFloor(Floor floor)
        {
            this.floor = floor;
            floor.shafts.Add(this);
        }

        /// <summary>
        /// Get the Y coordinate of the bottom
        /// </summary>
        /// <returns>Bottom as a Y coordinate</returns>
        private float GetBottom()
        {
            return this.position.Y + this.texture.Height / this.numberOfColumns;
        }
    }
}
