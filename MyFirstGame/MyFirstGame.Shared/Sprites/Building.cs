﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Sprites.Testing;

namespace MyFirstGame.Sprites
{
    /// <summary>
    /// Building. To build floors on top of.
    /// </summary>
	public class Building:BaseSprite
	{
		/// <summary>
        /// THE BOTTOM IS ALWAYS ZERO. THIS IS OUR BASIS. But we can override it.
        /// </summary>
        public float bottom = 0;

        /// <summary>
        /// Y coordinate of top of building
        /// </summary>
        public float top;

        /// <summary>
        /// Y coordinate of top of shaft
        /// </summary>
        public float shaftTop;
        
        /// <summary>
        /// List of all our floors
        /// </summary>
        public List<Floor> floors = new List<Floor>();

        /// <summary>
        /// The whole elevator shaft
        /// </summary>
        public List<ShaftSegment> shaft = new List<ShaftSegment>();

        /// <summary>
        /// Initializes the building
        /// </summary>
		public Building () : base(ContentLoader.GetTexture("groundLevel.png"), new Vector2(0, 600))
		{
            //this.origin = new Vector2(400, 0);
            this.top = this.position.Y;
            this.shaftTop = this.position.Y;
		}

        /// <summary>
        /// The texture alone determines the floor's position and everything.
        /// </summary>
        /// <param name="texture">Texture for floor</param>
        /// <param name="x">X axis position</param>
        public void AddFloor(Texture2D texture, float x = 0)
        {
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
            TestSpriteLayeringInitiate(newFloor);
        }

        /// <summary>
        /// TEST METHOD. REMOVE ONCE DONE
        /// </summary>
        private void TestSpriteLayeringInitiate(Floor f)
        {
            f.People = new MultiLayerSprite();
            f.People.sprites.Add(new TestPerson(f, new Vector2(100, 100)));
            f.People.sprites.Add(new TestPerson(f, new Vector2(110, 105)));
            f.People.sprites.Add(new TestPerson(f, new Vector2(120, 110)));
            f.People.sprites.Add(new TestPerson(f, new Vector2(130, 115)));
            /*f.People.sprites.Add(new BaseSprite(ContentLoader.GetTexture("blackball.png"),
                                new Vector2(110, 105)));
            f.People.sprites.Add(new BaseSprite(ContentLoader.GetTexture("blackball.png"),
                                new Vector2(120, 110)));
            f.People.sprites.Add(new BaseSprite(ContentLoader.GetTexture("blackball.png"),
                                new Vector2(100, 100)));
            f.People.sprites.Add(new BaseSprite(ContentLoader.GetTexture("blackball.png"),
                                new Vector2(130, 115)));*/
        }

        /// <summary>
        /// Adds a level of the shaft.
        /// </summary>
        /// <param name="sprite">Animated sprite to use for the shaft</param>
        /// <param name="x">X position</param>
        public void AddShaft(Texture2D texture, float x = 350, int numberOfColumns = 5, int numberOfRows = 1)
        {
            float height = texture.Height / numberOfRows;
            this.shaftTop = this.shaftTop - height;
            float y = this.shaftTop;

            ShaftSegment newShaft = new ShaftSegment(texture, new Vector2(x, y), numberOfColumns, numberOfRows);

            if (this.shaft.Count == 0)
            {
                this.shaft.Add(newShaft);
            }
            else
            {
                this.shaft[this.shaft.Count - 1].above = newShaft;
                newShaft.below = this.shaft[this.shaft.Count - 1];
                this.shaft.Add(newShaft);
            }
            newShaft.addAnimation(0, 0, newShaft.numberOfColumns-1, 0, 0.25f, "animation");
            newShaft.SetAnimation("animation");
            newShaft.BindToFloor(this.floors);
            this.screen.AddSprite(newShaft);
        }

        /// <summary>
        /// Overrides setting scale by also changing the top
        /// </summary>
        /// <param name="newScale">New scale to multiply by</param>
        public override void SetScale(float newScale)
        {
            base.SetScale(newScale);
            this.top = this.top * newScale;
            this.shaftTop = this.shaftTop * newScale;
        }
	}
}

