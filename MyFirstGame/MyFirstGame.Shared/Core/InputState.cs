using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstGame
{
    /// <summary>
    /// InputState contains static methods to handle all input
    /// </summary>
	public static class InputState
	{
        /// <summary>
        /// Current Keyboard state
        /// </summary>
		private static KeyboardState currentKeyboardState;

        /// <summary>
        /// Previous Keyboard state
        /// </summary>
		private static KeyboardState previousKeyboardState;

        /// <summary>
        /// Duration which key has been down for, in seconds
        /// </summary>
		private static Dictionary<Keys, double> durations = new Dictionary<Keys, double>();
        
        /// <summary>
        /// Checks if a key is pressed
        /// </summary>
        /// <param name="key">The key we want to check</param>
        /// <returns>True if that key is pressed, false otherwise</returns>
		public static bool IsKeyDown(Keys key) {
			return currentKeyboardState.IsKeyDown (key);
		}

        /// <summary>
        /// Checks if any key is pressed
        /// </summary>
        /// <returns>True if a key is pressed, false otherwise</returns>
		public static bool AnyKeyDown() {
			return durations.Skip (1).Sum (x => x.Value) > 0;
		}
			
        /// <summary>
        /// Checks if a key has been down for more than a duration
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="duration">Duration in seconds</param>
        /// <returns>True if key has been down for more than duration, false otherwise</returns>
		public static bool IsKeyDownFor(Keys key, double duration) { 
			return durations [key] > duration;
		}

        /// <summary>
        /// Checks if a key is up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is up, false otherwise</returns>
		public static bool IsKeyUp(Keys key) {
			return currentKeyboardState.IsKeyUp (key);
		}

        /// <summary>
        /// Checks if a key has just been pressed in this update cycle
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if key has been pressed this update cycle, false otherwise</returns>
		public static bool IsKeyPress(Keys key) {
			return currentKeyboardState.IsKeyDown (key) && previousKeyboardState.IsKeyUp (key);
		}

		/// <summary>
        /// Checks if a key has just been released in this update cycle
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if key has been released this update cycle, false otherwise</returns>
        public static bool IsKeyRelease(Keys key) {
			return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
		}

		/// <summary>
        /// Called at the start of every update cycle to determine the keyboard state
        /// </summary>
        /// <param name="gameTime">Game time</param>
		public static void BeginUpdate(GameTime gameTime) {
			currentKeyboardState = Keyboard.GetState();

			// Iterate through all possible keys and put durations into dictionary
			foreach (Keys key in Enum.GetValues(typeof(Keys)).Cast<Keys>()) {
				// Ignore the option for where Key is None
				if (key != Keys.None) { 
					if (currentKeyboardState.IsKeyDown (key)) {
						durations [key] += gameTime.ElapsedGameTime.TotalSeconds;
					} else
						durations [key] = 0;
				}
			}
		}

		/// <summary>
		/// Called at the end of every update cycle, to store the previous keyboard state
		/// </summary>
		public static void EndUpdate() {
			previousKeyboardState = currentKeyboardState;
		}



	}
}

