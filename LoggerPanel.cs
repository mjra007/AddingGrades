using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AddinGrades
{
    public interface ILogger {
        void WriteLineToPanel(string input);
        void Write(string input);

        void ErasePanel();
    }

    [ComVisible(true)]
    [ComDefaultInterface(typeof(ILogger))]
    [Guid("E2197CEB-6ADC-4EAB-80FD-0A9EE161BA14")]
    public class LoggerPanel : UserControl, ILogger
    {
        private RichTextBox console;

        public LoggerPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.console = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // console
            // 
            this.console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.console.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.console.Location = new System.Drawing.Point(0, 0);
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.Size = new System.Drawing.Size(1229, 151);
            this.console.TabIndex = 0; 
            // 
            // LoggerPanel
            // 
            this.Controls.Add(this.console);
            this.Name = "LoggerPanel";
            this.Size = new System.Drawing.Size(1229, 151);
            this.ResumeLayout(false); 

        }

        public void WriteLineToPanel(string input)
        {
            if (string.IsNullOrEmpty(console.Text))
            {
                console.Text += DateTime.Now.ToString()+": " + input;
            }
            else
            {
                console.Text += "\n" + DateTime.Now.ToString() + ": " + input;
            }
        }

        public void Write(string input)=> console.Text+=input; 

        public void ErasePanel() => console.Text = string.Empty;
    }
}
