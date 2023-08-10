using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddinGrades
{
    public interface IMyUserControl { }

    [ComVisible(true)]
    [ComDefaultInterface(typeof(IMyUserControl))]
    public class CustomPanel : UserControl, IMyUserControl
    {
        public Label TheLabel;
        public CustomPanel()
        {
            Controls.Add(TheLabel);
        }
    }
}
