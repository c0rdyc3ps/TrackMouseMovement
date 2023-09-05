using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TrackMouseMovement
{
    class Program
    {
        // P/Invoke declarations for cursor position tracking
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT Point);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        // Structs for coordinate positions
        public struct POINT
        {
            public Int32 X;
            public Int32 Y;
        }

        // Main method
        static public void Main(String[] args)
        {
            GetMouseMovement();
            Console.WriteLine("\n[+] Press any key to exit...");
            Console.ReadKey();
        }

        // Mouse movement tracking logic
        static void GetMouseMovement()
        {
            Console.WriteLine("\n[+] Tracking mouse movements...");

            // Declaring POINT variables and creating a list of tracked X and Y coordinates
            POINT current_position, previous_position;
            List<POINT> coordinates = new List<POINT>();

            // Setting each point starting position to zero
            previous_position.X = 0;
            previous_position.Y = 0;

            // Creating a new stopwatch
            var stopwatch = new Stopwatch();

            // Making a do/while loop
            do
            {
                // Starting the timer
                stopwatch.Start();

                // Logic to exit program execution before reaching payload logic if no user interation is detected
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    Console.WriteLine("\n    [-] More than 10 seconds has elapsed!");
                    Console.WriteLine("    [-] No user interaction detected; Buh Bye!");
                    break;
                }

                // Logic to track number of cursor movements to predict normal user interation
                if (GetCursorPos(out current_position))
                {
                    // Tracking number of mouse movements; if 200 total movements are detcted, the payload executes
                    int movements = coordinates.Count;
                    if (movements > 200)
                    {
                        // Payload execution
                        Process.Start("calc.exe");
                        break;
                    }

                    // Logic to print out mouse coordinates on the screen with number of movements and restart the stopwatch 10 second countdown of no interaction
                    if ((current_position.X != previous_position.X) || current_position.Y != previous_position.Y)
                    {
                        stopwatch.Stop();
                        Console.WriteLine($"    ({current_position.X},{current_position.Y}) : {stopwatch.ElapsedMilliseconds} ms between mouse movements; Total movements tracked: {coordinates.Count}");
                        coordinates.Add(current_position);
                        stopwatch.Restart();
                    }

                    // Updating cursor position
                    previous_position.X = current_position.X;
                    previous_position.Y = current_position.Y;
                }
            }
            // While condition for stopwatch timing
            while (stopwatch.ElapsedMilliseconds != 11000);
        }
    }
}
