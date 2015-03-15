using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyFirstGame
{
	public class InputState
	{
		private static KeyboardState ks; //current keyboard state
		private static KeyboardState pks; //previous keyboard state

		private static Dictionary<Keys, double> durations = new Dictionary<Keys, double>();

		public InputState ()
		{
			
		}

		public static bool IsDown(Keys key) {
			return ks.IsKeyDown (key);
		}

		public static bool AnyDown() {
			return durations.Skip (1).Sum (x => x.Value) > 0;
		}
			
		public static bool IsDownFor(Keys key, double duration) { 
			return durations [key] > duration;
		}

		public static bool IsUp(Keys key) {
			return ks.IsKeyUp (key);
		}

		public static bool IsPress(Keys key) {
			return ks.IsKeyDown (key) && pks.IsKeyUp (key);
		}

		public static bool IsRelease(Keys key) {
			return ks.IsKeyUp(key) && pks.IsKeyDown(key);
		}

		//Call this before all input checks
		public static void BeginUpdate(GameTime gameTime) {
			ks = Keyboard.GetState();
			//Iterate through all possible keys and put durations into dictionary
			foreach (Keys key in Enum.GetValues(typeof(Keys)).Cast<Keys>()) {
				//want to ignore none option
				if (key != Keys.None) { 
					if (ks.IsKeyDown (key)) {
						durations [key] += gameTime.ElapsedGameTime.TotalSeconds;
					} else
						durations [key] = 0;
				}
			}
		}

		//Call this after all input checks
		public static void EndUpdate() {
			pks = ks;
		}



	}
}

