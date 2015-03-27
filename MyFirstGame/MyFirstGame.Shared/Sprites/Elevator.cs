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
		public float acceleration = 150;

        /// <summary>
        /// Deacceleration
        /// </summary>
        public float deacceleration = 200;

		/// <summary>
		/// how fast the elevator accelerates when moving to the nearest floor
		/// </summary>
		public float homingMultiplier = 15f;

        /// <summary>
        /// Maximum speed
        /// </summary>
		public float maxSpeed = 300;

        /// <summary>
        /// Minimum speed
        /// </summary>
        public float minSpeed = 50;

        /// <summary>
        /// At this distance, we don't care about direction of movement.
        /// </summary>
        public float cutOffDistance = 50;

        /// <summary>
        /// Whether the elevator is being controlled
        /// </summary>
        public bool isControlled = false;

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
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public Elevator(float x = 0, float y = 0):
		base(ContentLoader.GetTexture("elevator.png"),
            new Vector2(x, y),
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
            this.TestAnimationKeys();

            // Handles moving up and down to change speed
            this.HandleInput(gameTime);

            // Sets the current floor that we're at
            this.SetCurrentFloor();
            if (InputState.AreKeysUp(Keys.Up, Keys.Down))
            {
                // Deaccelerates to minspeed if there's no input and there's no nearby floor
                this.Deaccelerate(gameTime);
            }
            // Changes direction of movement if we're near a floor
            this.ProjectToNearFloor(gameTime);

            // Now we can get the new position
            float newPositionY = this.position.Y + CurrentGame.GetDelta(gameTime) * this.currentSpeed;

            // Move if we haven't forced a position change
            // NOT A SHORT CIRCUIT. We must run both.
            // First condition binds position to elevator shaft/building so our elevator doesn't fly to space.
            // Second condition determines if we are crossing a floor boundary, and stops if conditions are right.
            // If one is true, that means we have forced a positional change, and should not take the new position.
            if (!this.BindPosition(newPositionY) & !this.StopAtFloor(newPositionY))
            {
                this.position.Y = newPositionY;
            }

            // If we have 0 speed and are still, open the door
            if (this.currentState == AnimationNames.Still && this.currentSpeed == 0)
            {
                this.SetStateAndAnimation(AnimationNames.Opening);
                this.fade = 1.0f;
                //CurrentGame.camera.targetScale = 1.0f;
            }
            // Otherwise, do stuff like zoom in and all that
            else if (this.currentState == AnimationNames.Accelerating)
            {
                this.fade = 0.5f;
                //CurrentGame.camera.targetScale = 1.75f;
            }
        }

        /// <summary>
        /// Changes direction of movement if we're at minimum speed
        /// </summary>
        /// <param name="gameTime">Game time to calculate time elapsed</param>
        /// <returns>True if near to a floor and at minimum speed, false otherwise</returns>
        private bool ProjectToNearFloor(GameTime gameTime)
        {
            // First condition: No control being placed, and we are deaccelerating
            if (!this.isControlled && this.currentState == AnimationNames.Deaccelerating)
            {
                // Next condition: Speed must be within minimum speed
                if (Math.Abs(this.currentSpeed) <= this.minSpeed)
                {
                    float elevatorHeight = this.texture.Height / this.numberOfRows;

                    // Case for when we need to go down
                    if (this.currentFloor.bottom - (this.position.Y + elevatorHeight) < this.cutOffDistance)
                    {
                        this.currentSpeed = this.minSpeed;
                        return true;
                    }

                    // Case for when we need to go up
                    else if (this.currentFloor.upstairs != null && ((this.position.Y + elevatorHeight) - this.currentFloor.upstairs.bottom < this.cutOffDistance))
                    {
                        this.currentSpeed = -this.minSpeed;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Binds the elevator to not move out of the elevator shaft
        /// </summary>
        /// <param name="newPositionY">The intended new position we want to move the elevator to</param>
        /// <remarks>True if we forced a change in position, false otherwise</remarks>
        private bool BindPosition(float newPositionY)
        {
            float elevatorHeight = this.texture.Height / this.numberOfRows;

            // Bind elevator to building. TODO: Bind to shaft instead.
            if (newPositionY <= this.CurrentBuilding.shaftTop)
            {
                this.position.Y = this.CurrentBuilding.shaftTop;
                this.currentSpeed = 0;
                return true;
            }
            else if (newPositionY + elevatorHeight > this.CurrentBuilding.position.Y)
            {
                this.position.Y = this.CurrentBuilding.position.Y - elevatorHeight;
                this.currentSpeed = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stops at floor if we're going past it at a low speed
        /// </summary>
        /// <param name="newPositionY"></param>
        /// <returns></returns>
        private bool StopAtFloor(float newPositionY)
        {
            // First condition: No control being placed, and we are deaccelerating
            if (!this.isControlled && this.currentState == AnimationNames.Deaccelerating)
            {
                // Next condition: Speed must be within minimum speed
                if (Math.Abs(this.currentSpeed) <= this.minSpeed)
                {
                    // Finally, if we're crossing a boundary. Recall that the currentfloor is already set
                    float elevatorHeight = this.texture.Height / this.numberOfRows;
                    float newBottom = newPositionY + elevatorHeight;
                    
                    // The case for when we're going down
                    if (newBottom >= this.currentFloor.bottom)
                    {
                        this.currentSpeed = 0;
                        this.SetStateAndAnimation(AnimationNames.Still);
                        this.position.Y = this.currentFloor.bottom - elevatorHeight;
                        return true;
                    }

                    // The case for when we're going up
                    else if (this.currentFloor.upstairs != null && newBottom <= this.currentFloor.upstairs.bottom)
                    {
                        this.currentSpeed = 0;
                        this.SetStateAndAnimation(AnimationNames.Still);
                        this.position.Y = this.currentFloor.upstairs.bottom - elevatorHeight;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///  Sets the floor that the player is currently at. To hit a floor, you must be above, or exactly at, that floor
        /// </summary>
        private void SetCurrentFloor()
        {
            float elevatorHeight = this.texture.Height / this.numberOfRows;
            float currentBottom = this.position.Y + elevatorHeight;

            // Change to a lower floor
            while (currentBottom > this.currentFloor.bottom && this.currentFloor.downstairs != null)
            {
                this.ToggleFloorCurrent(false);
                this.currentFloor = this.currentFloor.downstairs;
                this.ToggleFloorCurrent(true);
            }
            // Change to a higher floor
            while (currentBottom <= this.currentFloor.position.Y && this.currentFloor.upstairs != null)
            {
                this.ToggleFloorCurrent(false);
                this.currentFloor = this.currentFloor.upstairs;
                this.ToggleFloorCurrent(true);
            }
        }

        /// <summary>
        /// Toggles some floors as current, for art and other stuff
        /// </summary>
        /// <param name="toggle">Set to true or false</param>
        private void ToggleFloorCurrent(bool toggle)
        {
            if (this.currentFloor.upstairs != null)
            {
                this.currentFloor.upstairs.Current = toggle;
            }
            if (this.currentFloor.downstairs != null)
            {
                this.currentFloor.downstairs.Current = toggle;
            }
            this.currentFloor.Current = toggle;
        }

        /// <summary>
        /// Deaccelerates the elevator.
        /// </summary>
        /// <param name="gameTime">Game time to calculated elapsed time</param>
        private void Deaccelerate(GameTime gameTime)
        {

            // Actions to perform upon letting go
            if (this.isControlled)
            {
                this.isControlled = false;
            }

            if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving || this.currentState == AnimationNames.Deaccelerating)
            {
                float delta = CurrentGame.GetDelta(gameTime);
                float elevatorHeight = this.texture.Height / this.numberOfRows;
                float newSpeed = this.currentSpeed;

                // Deaccelerate if greater than min speed
                if (this.currentSpeed > this.minSpeed)
                {
                    newSpeed = this.currentSpeed - delta * this.deacceleration;
                }
                else if (this.currentSpeed < -this.minSpeed)
                {
                    newSpeed = this.currentSpeed + delta * this.deacceleration;
                }

                // Set to min speed if less than min speed.
                if (this.currentSpeed < this.minSpeed && this.currentSpeed > 0)
                {
                    newSpeed = Math.Min(this.currentSpeed + delta * this.deacceleration, this.minSpeed);
                }
                else if (this.currentSpeed > -this.minSpeed && this.currentSpeed < 0)
                {
                    newSpeed = Math.Max(this.currentSpeed - delta * this.deacceleration, -this.minSpeed);
                }

                // Set animation to decelerating when speed starts decreasing or in the special case where the top or bottom of building is hit
                if ((Math.Abs(this.previousSpeed) < Math.Abs(this.currentSpeed) || Math.Abs(this.currentSpeed) == this.minSpeed || this.currentSpeed == 0) && this.currentState != AnimationNames.Deaccelerating)
                {
                    this.SetStateAndAnimation(AnimationNames.Deaccelerating);
                }
                this.currentSpeed = newSpeed;

                this.previousSpeed = this.currentSpeed;
            }
        }

        /// <summary>
        /// Handles input for moving the elevator when keys are up or down
        /// </summary>
        /// <param name="gameTime">Game time to calculate elapsed time</param>
        private void HandleInput(GameTime gameTime)
        {
            if (InputState.IsKeyDown(Keys.Up))
            {
                currentKey = Keys.Up;
				if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving)
                {
                    this.currentSpeed = this.currentSpeed - CurrentGame.GetDelta(gameTime) * this.acceleration;
                }

                if (this.currentState == AnimationNames.Opened)
                {
                    this.SetStateAndAnimation(AnimationNames.Closing);
                }

                if (this.currentState == AnimationNames.Still || this.currentState == AnimationNames.Deaccelerating)
                {
                    this.SetStateAndAnimation(AnimationNames.Accelerating);
                }
                this.isControlled = true;
            }

            if (InputState.IsKeyDown(Keys.Down))
            {
                currentKey = Keys.Down;
				if (this.currentState == AnimationNames.Accelerating || this.currentState == AnimationNames.Moving)
                {
                    this.currentSpeed = this.currentSpeed + CurrentGame.GetDelta(gameTime) * acceleration;
                }

                if (this.currentState == AnimationNames.Opened)
                {
                    this.SetStateAndAnimation(AnimationNames.Closing);
                }

                if (this.currentState == AnimationNames.Still || this.currentState == AnimationNames.Deaccelerating)
                {
                    this.SetStateAndAnimation(AnimationNames.Accelerating);
                }
                this.isControlled = true;
            }
            this.currentSpeed = MathHelper.Clamp(this.currentSpeed, -1 * this.maxSpeed, this.maxSpeed);
        }

        /// <summary>
        /// REMOVE THIS SOON.
        /// Just shortcuts to test animations.
        /// </summary>
        private void TestAnimationKeys()
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
