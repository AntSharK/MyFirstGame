﻿using System;
using System.Collections.Generic;
using System.Text;
using MyFirstGame.Screens;
using Microsoft.Xna.Framework;

namespace MyFirstGame
{
    /// <summary>
    /// Utility methods for game class are stored here
    /// </summary>
    public partial class GameRunner : Game
    {
        /// <summary>
        /// Adds a screen to the game
        /// </summary>
        /// <param name="screen">Screen to be added</param>
        /// <param name="screenName">Name of screen in screenDictionary</param>
        private void addScreen(BaseScreen screen, string screenName)
        {
            screens.AddLast(screen);
            screenDictionary.Add(screenName, screen);
        }

        /// <summary>
        /// Remove a screen using its name
        /// </summary>
        /// <param name="screenName">Screen name to be removed</param>
        /// <returns>True if screen was successfully removed, false if not</returns>
        private bool removeScreen(string screenName)
        {
            BaseScreen toBeRemoved;
            try
            {
                screenDictionary.TryGetValue(screenName, out toBeRemoved);
                screens.Remove(toBeRemoved);
                screenDictionary.Remove(screenName);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}