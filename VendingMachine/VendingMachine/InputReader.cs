using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace VendingMachine
{
    public class InputReader
    {
        public event EventHandler KeyPressed;
        public char PressedKey = '-';
        private bool ReadingInputs = false;

        public InputReader() { }

        public void Activate() { ReadingInputs = true; ReadInputs(); }
        public void Deactivate() { ReadingInputs = false; }

        public void ReadInputs()
        {
            while (ReadingInputs)
            {
                PressedKey = ReadKey().KeyChar;
                KeyPressed.Invoke(this, new EventArgs());
            }
        }
    }
}
