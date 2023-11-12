using System.Runtime.InteropServices; 

namespace AddinGrades
{
    public interface ILogger
    {
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
            console = new RichTextBox();
            SuspendLayout();
            // 
            // console
            // 
            console.Dock = DockStyle.Fill;
            console.Font = new Font("Calibri", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            console.Location = new Point(0, 0);
            console.Name = "console";
            console.ReadOnly = true;
            console.Size = new Size(1229, 151);
            console.TabIndex = 0;
            console.Text = "";
            // 
            // LoggerPanel
            // 
            Controls.Add(console);
            Name = "LoggerPanel";
            Size = new Size(1229, 151);
            ResumeLayout(false);
        }

        public void WriteLineToPanel(string input)
        {
            if (string.IsNullOrEmpty(console.Text))
            {
                console.Text += DateTime.Now.ToString() + ": " + input;
            }
            else
            {
                console.Text += "\n" + DateTime.Now.ToString() + ": " + input;
            }
        }

        public void Write(string input) => console.Text += input;

        public void ErasePanel() => console.Text = string.Empty;
    }
}
