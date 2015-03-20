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
    public class Elevator : BaseAnimatedSprite
    {
        /// <summary>
        /// Animation names. Const strings are used so it's easier to reference proper names.
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

        /// <summary>
        /// Current y velocity. Positive is DOWN.
        /// </summary>
        public float currentSpeed = 0;

        /// <summary>
		/// y velocity in previous frame
		/// </summary>
		float previousSpeed;

        /// <summary>
        /// Factor to deaccelerate by each interval
        /// </summary>
        public float deacceleratePerSecond = 0.95f;
	
        /// <summary>
        /// Acceleration
        /// </summary>
		public float acceleration = 1500;

		/// <summary>
		/// how fast the elevator accelerates when moving to the nearest floor
		/// </summary>
		public float homingMultiplier = 15f;

        /// <summary>
        /// Maximum speed
        /// </summary>
		public float maxSpeed = 300;

        /// <summary>
        /// Cutoff before setting speed to 0
        /// </summary>
        public float cutOff = 0.2f;

        /// <summary>
        /// Current key being pressed
        /// </summary>
		public Keys currentKey;

		/// <summary>
		/// Closest floor in moving direction
		/// </summary>
		public float destinationY;

        /// <summary>
        /// Current building being operated on
        /// </summary>
		private Building currentBuilding;

        /// <summary>
        /// Floor currently at
        /// </summary>
        public Floor currentFloor;

        /// <summary>
        /// Property to get or set current building
        /// </summary>
        public Building CurrentBuilding
        {
            get
            {
                return this.currentBuilding;
            }
            set
            {
                this.currentBuilding = value;

                // Set to the ground floor
                this.currentFloor = this.currentBuilding.floors[0];
                this.position.Y = this.currentFloor.bottom - this.texture.Height/this.numberOfRows;
            }
        }
	
        /// <summary>
        /// Initializes a new instance of the test animated sprite
        /// </summary>
        public Elevator():
		base(ContentLoader.GetTexture("elevator.png"),
            new Vector2(0, 0),
            6, 1)
        {
            this.addAnimation(0, 0, 0, 0, 1f, AnimationNames.Still);
            this.addAnimation(0, 0, 5, 0, 0.1f, AnimationNames.Opening, false, false, () => this.SetStateAndAnimation(AnimationNames.Opened));
            this.addAnimation(5, 0, 0, 0, 0.1f, AnimationNames.Closing, false, false, () => this.SetStateAndAnimation(AnimationNames.Still));
            this.addAnimation(5, 0, 5, 0, 1f, AnimationNames.Opened);
			this.addAnimation (0, 0, 0, 0, 0.1f, AnimationNames.Accelerating, false, false, () => this.SetStateAndAnimation (AnimationNames.Moving));
			this.addAnimation (0, 0, 0, 0, 0.15f, AnimationNames.Deaccelerating, false, false);//, () => this.SetStateAndAnimation(AnimationNames.Still));
            this.addAnimation(0, 0, 0, 0, 0.15f, AnimationNames.Moving);

            this.SetStateAndAnimation(AnimationNames.Still);
        }

        /// <summary>
        /// Updates stuff.
        /// REMEMBER TO CALL BASE.UPDATE to draw things.
        /// </summary>
        /// <param name="gameTime">GameTime from main game</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Test method for registering animations
            this.testAnimationInput();

            // Handles moving up and down
            this.handleMovingInput(gameTime);
            
            // Goes to the floor we think the player is headed to.
            this.projectToIntendedFloor(gameTime);
			
            // Cut off speed to 0 if we've deaccelerated enough
			if (this.currentState == AnimationNames.Deaccelerating) {
				this.currentSpeed = this.currentSpeed * this.deacceleratePerSecond;
				if ((this.currentSpeed < this.cutOff && this.currentSpeed <= 0) || (this.currentSpeed > -this.cutOff && this.currentSpeed >= 0)) {
					this.currentSpeed = 0;
					this.SetStateAndAnimation (AnimationNames.Still);
				}
			}

            // If we have 0 speed and are still, open the door
            if (this.currentState == AnimationNames.Still)
            {
				this.currentSpeed = 0;
                this.SetStateAndAnimation(AnimationNames.Opening);
				//CurrentGame.camera.targetScale = 1;
            }

			this.currentSpeed = MathHelper.Clamp (this.currentSpeed, -1 * this.maxSpeed, this.maxSpeed);
			if (this.currentSpeed != 0) {
				//CurrentGame.camera.targetScale = 1.5f;
			}

            // We project the position, to not hit one pixel off
            float newPositionY = this.position.Y + CurrentGame.getDelta(gameTime) * currentSpeed;
            float elevatorHeight = this.texture.Height / this.numberOfRows;

            // Bind elevator to building. TODO: Bind to shaft instead.
            
            if (newPositionY < this.CurrentBuilding.top)
            {
                this.position.Y = this.CurrentBuilding.top;
            }
            else if (newPositionY + elevatorHeight > this.CurrentBuilding.position.Y)
            {
                this.position.Y = this.CurrentBuilding.position.Y - elevatorHeight;
            }
            else
            {
                // Change the position of the elevator
                this.position.Y = newPositionY;
                float elevatorBottom = this.position.Y + elevatorHeight;

                // Change the current floor. While loop because we might go past multiple floors in one update
                while (this.currentSpeed > 0 && elevatorBottom > this.currentFloor.bottom && this.currentFloor.downstairs != null)
                    this.currentFloor = this.currentFloor.downstairs;
                while (this.currentSpeed < 0 && elevatorBottom < this.currentFloor.bottom && this.currentFloor.upstairs != null)
                    this.currentFloor = this.currentFloor.upstairs;
            }
        }

        /// <summary>
        /// Projects the elevator to the intended floor.
        /// </summary>
        /// <param name="gameTime">Game time to calculated elapsed time</param>
        private void projectToIntendedFloor(GameTime gameTime)
        {
            if (InputState.AreKeysUp(Keys.Up, Keys.Down))
            {
                if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving)
                {
                    /*
                    if (this.currentSpeed != 0)
                    {
                        // Find the closest floor in the moving direction
                        float closestFloor = float.MaxValue, smallestDist = float.MaxValue;
                        float elevatorHeight = this.texture.Height / this.numberOfRows;
						float elevatorBottom = this.position.Y + elevatorHeight;
                        
						for (int i = 0; i < currentBuilding.floors.Count; i++)
                        {
							if ((Math.Sign(this.currentBuilding.floors[i].bottom - elevatorBottom) ==  Math.Sign(this.currentSpeed) || this.currentBuilding.floorBase[i] - elevatorBottom == 0) &&
								Math.Abs(this.currentBuilding.floors[i].bottom - elevatorBottom) < smallestDist)
                            {
								smallestDist = Math.Abs(currentBuilding.floors[i].bottom - elevatorBottom);
								closestFloor = this.currentBuilding.floors[i].bottom;
                            }
                        }
						destinationY = closestFloor - this.texture.Height / this.numberOfRows;
                        
                        //destinationY = this.currentFloor.bottom - elevatorHeight;
                    }
                    */
                    float delta = CurrentGame.getDelta(gameTime);
                    float elevatorHeight = this.texture.Height / this.numberOfRows;
                    this.destinationY = this.currentFloor.bottom - elevatorHeight;
                    // Set the speed based on the floor location
					this.currentSpeed = MathHelper.Clamp((destinationY - this.position.Y) * homingMultiplier, -1 * maxSpeed, maxSpeed);

					// Check if floor has been reached
					if (Math.Abs(this.destinationY - (this.position.Y + this.currentSpeed*delta)) < 1)
                    {
                        this.position.Y = destinationY;

                        this.currentState = AnimationNames.Deaccelerating;
                    }

					//Set animation to decelerating when speed starts decreasing
					if (Math.Abs (this.previousSpeed) < Math.Abs (this.currentSpeed) && this.currentState != AnimationNames.Deaccelerating) {
						this.SetAnimation(AnimationNames.Deaccelerating);
					}

					this.previousSpeed = this.currentSpeed;
                }
            }
        }

        /// <summary>
        /// Handles input for moving the elevator when keys are up or down
        /// </summary>
        /// <param name="gameTime">Game time to calculate elapsed time</param>
        private void handleMovingInput(GameTime gameTime)
        {
            if (InputState.IsKeyDown(Keys.Up))
            {
                currentKey = Keys.Up;
				if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving)
                {
                    this.currentSpeed = this.currentSpeed - CurrentGame.getDelta(gameTime) * this.acceleration;
                }

                if (this.currentState == AnimationNames.Opened)
                {
                    this.SetStateAndAnimation(AnimationNames.Closing);
                }

                if (this.currentState == AnimationNames.Still)
                {
                    this.SetStateAndAnimation(AnimationNames.Accelerating);
                }
            }

            if (InputState.IsKeyDown(Keys.Down))
            {
                currentKey = Keys.Down;
				if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving)
                {
                    this.currentSpeed = this.currentSpeed + CurrentGame.getDelta(gameTime) * acceleration;
                }

                if (this.currentState == AnimationNames.Opened)
                {
                    this.SetStateAndAnimation(AnimationNames.Closing);
                }

                if (this.currentState == AnimationNames.Still)
                {
                    this.SetStateAndAnimation(AnimationNames.Accelerating);
                }
            }
        }

        /// <summary>
        /// REMOVE THIS SOON.
        /// Just shortcuts to test animations.
        /// </summary>
        private void testAnimationInput()
        {
            if (InputState.IsKeyDown(Keys.Z))
            {
                this.isVisible = !this.isVisible;
            }
            if (InputState.IsKeyDown(Keys.X))
            {
                this.SetStateAndAnimation(AnimationNames.Opening);
            }
            if (InputState.IsKeyDown(Keys.C))
            {
                this.SetStateAndAnimation(AnimationNames.Closing);
            }
            if (InputState.IsKeyDown(Keys.V))
            {
                this.SetStateAndAnimation(AnimationNames.Still);
            }
            if (InputState.IsKeyDown(Keys.B))
            {
                this.SetStateAndAnimation(AnimationNames.Opened);
            }
            if (InputState.IsKeyDown(Keys.N))
            {
                this.SetStateAndAnimation(AnimationNames.Accelerating);
            }
            if (InputState.IsKeyDown(Keys.M))
            {
                this.SetStateAndAnimation(AnimationNames.Deaccelerating);
            }
            if (InputState.IsKeyDown(Keys.OemComma))
            {
                this.SetStateAndAnimation(AnimationNames.Moving);
            }
        }
    }
}
