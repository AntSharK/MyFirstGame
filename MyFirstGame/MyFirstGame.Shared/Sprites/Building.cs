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
		public Building () : base(ContentLoader.GetTexture("groundLevel.png"), new Vector2(400, 600))
		{
            this.origin = new Vector2(400, 0);
            this.top = 0;
		}

        /// <summary>
        /// The texture alone determines the floor's position and everything.
        /// </summary>
        /// <param name="texture"></param>
        public void addFloor(Texture2D texture)
        {
            // 100 to account for space for shaft.
            float x = 100 + texture.Width/2;
            float y = this.top + texture.Height/2;
            Floor newFloor = new Floor(texture, new Vector2(x, y));
            this.top = this.top + texture.Height;

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
        /*
        /// <summary>
        /// Draw the base, then draw all floors
        /// </summary>
        /// <param name="batch"></param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            foreach(Floor floor in this.floors)
            {
                floor.Draw(batch);
            }
        }
        */
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

