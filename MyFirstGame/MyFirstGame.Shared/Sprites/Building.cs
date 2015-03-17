using System;
using MyFirstGame.Sprites;
using Microsoft.Xna.Framework;

namespace MyFirstGame
{
	public class Building:BaseSprite
	{
		public float[] floors = {200, 400, 600};
		public Building (): base(ContentLoader.GetTexture("building.png"), new Vector2(100, 0))
		{
			
		}

	}
}

