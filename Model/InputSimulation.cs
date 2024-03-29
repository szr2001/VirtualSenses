using System.Runtime.InteropServices;
using Point = System.Drawing.Point;

namespace ImageDetection
{
    //temp class
    public static class InputSimulation
    {
        //importing the dll and the needed methods

        //send input
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        //read mouse pos
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        //all keys and their id
        private static Dictionary<string, ushort> keycodes = new Dictionary<string, ushort>()
        {
            {"a", 0x41},
            {"b", 0x42},
            {"c", 0x43},
            {"d", 0x44},
            {"e", 0x45},
            {"f", 0x46},
            {"g", 0x47},
            {"h", 0x48},
            {"i", 0x49},
            {"j", 0x4A},
            {"k", 0x4B},
            {"l", 0x4C},
            {"m", 0x4D},
            {"n", 0x4E},
            {"o", 0x4F},
            {"p", 0x50},
            {"q", 0x51},
            {"r", 0x52},
            {"s", 0x53},
            {"t", 0x54},
            {"u", 0x55},
            {"v", 0x56},
            {"w", 0x57},
            {"x", 0x58},
            {"y", 0x59},
            {"z", 0x5A}

        };

        //asign the key codes for keyboard events
        private const int INPUT_KEYBOARD = 1;

        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        //asign the key codes for mouse events
        private const int INPUT_MOUSE = 0;

        //mose event ids
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        //create a point to store thelocation of a point on the screen
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        //the class that stores the new input that will be executed
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint Type;
            public INPUTUNION Data;
        }


        //holds the data for mouse and keyboard input
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
        }

        //mouse input, location, timestamp , flags if its button up or down
        private struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        //keyboard input vk means keykode, flags if key up or down
        private struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
        }
        public enum ButtonType
        {
            LeftButton,
            RightButton
        }
        //simulate key press 
        public static void KeyPress(string Key)
        {
            if (!keycodes.ContainsKey(Key))
            {
                Console.WriteLine("Error,Key dosent exists");
                return;
            }

            INPUT input = new INPUT();
            input.Type = INPUT_KEYBOARD;
            input.Data.Keyboard.Vk = keycodes[Key];
            input.Data.Keyboard.Scan = 0;
            input.Data.Keyboard.Flags = KEYEVENTF_KEYDOWN;
            input.Data.Keyboard.Time = 0;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT)));

            // Simulate key release
            input.Data.Keyboard.Flags = KEYEVENTF_KEYUP;
            SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT)));
        }
        //simulate mouse click 
        public static void MouseClick(Point p, ButtonType button)
        {

            uint Up;
            uint Down;

            //asigns the correct id for the requested button
            if (button == ButtonType.LeftButton)
            {
                Up = MOUSEEVENTF_LEFTUP;
                Down = MOUSEEVENTF_LEFTDOWN;
            }
            else
            {
                Up = MOUSEEVENTF_RIGHTUP;
                Down = MOUSEEVENTF_RIGHTDOWN;
            }

            INPUT[] Input = new INPUT[1];
            Input[0].Type = INPUT_MOUSE;
            Input[0].Data.Mouse.X = CalculateAbsoluteCoordinateX(p.X);//calculates abolute  X screen coords
            Input[0].Data.Mouse.Y = CalculateAbsoluteCoordinateY(p.Y);//calculates abolute  Y screen coords
            Input[0].Data.Mouse.MouseData = 0;


            Input[0].Data.Mouse.Flags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            SendInput(1, Input, Marshal.SizeOf(new INPUT()));//move mouse

            Input[0].Data.Mouse.Flags = Down;
            SendInput(1, Input, Marshal.SizeOf(new INPUT()));//press

            Input[0].Data.Mouse.Flags = Up;
            SendInput(1, Input, Marshal.SizeOf(new INPUT()));//release
        }
        
        //simulate a mouse pressing of a button
        public static void MousePress(Point p, ButtonType button)
        {
            uint Action;

            //asigns the correct id for the requested button
            if (button == ButtonType.LeftButton)
            {
                Action = MOUSEEVENTF_LEFTDOWN;
            }
            else
            {
                Action = MOUSEEVENTF_RIGHTDOWN;
            }

            INPUT[] Inputs = new INPUT[1];
            Inputs[0].Type = INPUT_MOUSE;
            Inputs[0].Data.Mouse.X = CalculateAbsoluteCoordinateX(p.X);
            Inputs[0].Data.Mouse.Y = CalculateAbsoluteCoordinateY(p.Y);
            Inputs[0].Data.Mouse.MouseData = 0;

            Inputs[0].Data.Mouse.Flags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            SendInput(1, Inputs, Marshal.SizeOf(typeof(INPUT)));//move

            Inputs[0].Data.Mouse.Flags = Action;
            SendInput(1, Inputs, Marshal.SizeOf(typeof(INPUT)));//press
        }
        //simulates a mouse release of a button
        public static void MouseRelease(Point p, ButtonType button)
        {
            uint Action;

            //asigns the correct id for the requested button
            if (button == ButtonType.LeftButton)
            {
                Action = MOUSEEVENTF_LEFTUP;
            }
            else
            {
                Action = MOUSEEVENTF_RIGHTUP;
            }

            INPUT[] Inputs = new INPUT[1];
            Inputs[0].Type = INPUT_MOUSE;
            Inputs[0].Data.Mouse.X = CalculateAbsoluteCoordinateX(p.X);
            Inputs[0].Data.Mouse.Y = CalculateAbsoluteCoordinateY(p.Y);
            Inputs[0].Data.Mouse.MouseData = 0;

            Inputs[0].Data.Mouse.Flags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            SendInput(1, Inputs, Marshal.SizeOf(typeof(INPUT)));//move

            Inputs[0].Data.Mouse.Flags = Action;
            SendInput(1, Inputs, Marshal.SizeOf(typeof(INPUT)));//release
        }
        public static Point GetMouseLoc()
        {
            GetCursorPos(out POINT p);
            Point point = new Point(p.X,p.Y);

            return point;
        }
        public static void SetCursorLoc(Point p)
        {
            SetCursorPos(p.X, p.Y);
        }
        private static int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }

        private static int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }
    }
}
