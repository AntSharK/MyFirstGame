using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Building. To build floors on top of.
    /// </summary>
	public class Building:BaseSprite
	{
		public float[] floorBase = {200, 400, 600};

        /// <summary>
        /// THE BOTTOM IS ALWAYS ZERO. THIS IS OUR BASIS.
        /// </summary>
        public const float bottom = 0;

        /// <summary>
        /// Y coordinate of top of building
        /// </summary>
        public float top;

        /// <summary>
        /// List of all our floors
        /// </summary>
        public List<Floor> floors = new List<Floor>();

        /// <summary>
        /// Initializes the building
        /// </summary>
		public Building () : base(ContentLoader.GetTexture("groundLevel.png"), new Vector2(0, 600))
		{
            //this.origin = new Vector2(400, 0);
            this.top = this.position.Y;
		}

        /// <summary>
        /// The texture alone determines the floor's position and everything.
        /// </summary>
        /// <param name="texture"></param>
        public void addFloor(Texture2D texture)
        {
            // 100 to account for space for shaft.
			float x = 100;
            this.top = this.top - texture.Height;
			float y = this.top;
            Floor newFloor = new Floor(texture, new Vector2(x, y));

            if (this.floors.Count == 0)
            {
                this.floors.Add(newFloor);
            }
            else
            {
                this.floors[this.floors.Count - 1].upstairs = newFloor;
                newFloor.downstairs = this.floors[this.floors.Count - 1];
                this.floors.Add(newFloor);
            }
            this.screen.AddSprite(newFloor);
        }

        /// <summary>
        /// Overrides setting scale by also changing the top
        /// </summary>
        /// <param name="newScale">New scale to multiply by</param>
        public override void setScale(float newScale)
        {
            base.setScale(newScale);
            this.top = this.top * newScale;
        }
	}
}

