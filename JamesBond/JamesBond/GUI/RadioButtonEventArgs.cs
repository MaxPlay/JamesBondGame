using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.GUI
{
    public class RadioButtonEventArgs : EventArgs
    {
        public string Language;

        public RadioButtonEventArgs(string lang)
        {
            this.Language = lang;
        }
    }
}
