using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyFirstGame.Sprites.Testing
{
    public class TestPerson : BaseSprite
    {
        public float moveDirection = 1;

        public Floor floor;

        public TestPerson(Floor f, Vector2 relativePos):
            base(ContentLoader.GetTexture("blackball.png"), 
            new Vector2(f.position.X + relativePos.X, f.position.Y + relativePos.Y))
        {
            this.floor = f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float delta = CurrentGame.GetDelta(gameTime);
            float newY = this.position.Y + delta * 100 * moveDirection;
            
            if (newY > floor.position.Y + floor.texture.Height || newY < floor.position.Y)
            {
                this.moveDirection = -this.moveDirection;
                newY = this.position.Y + delta * 100 * moveDirection;
            }
            this.position.Y = newY;
        }
    }
}
