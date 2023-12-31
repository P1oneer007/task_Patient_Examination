﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    static class WindowUtility
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static void FixeConsoleWindow(int windowHeight, int windowWidth)
        {
            try
            {
                Console.WindowHeight = windowHeight;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WindowHeight = Console.LargestWindowHeight - 4;
                Configuration.SetElementsOnPage((Console.WindowHeight - Configuration.InfoPanelHeight - Configuration.ComandPanelHeight) - 4);
            }

            try
            {
                Console.WindowWidth = windowWidth;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WindowWidth = Console.LargestWindowWidth;
            }

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }

        internal static void FixeConsoleWindow(object consoleHeight, object consoleWidth)
        {
            throw new NotImplementedException();
        }
    }
}
