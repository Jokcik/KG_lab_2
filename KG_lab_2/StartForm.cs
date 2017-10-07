using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace KG_lab_2
{
    public sealed class StartForm : Form
    {
        private TextBox funcTextBox;
        
        private void InitializeComponent()
        {
            Text = @"Лабораторная работа №2. Графики";
            Width = 300;
            Height = 300;
            Location = new Point(100, 200);
            StartPosition = FormStartPosition.Manual;

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Width = Width - 4 * 20
            };

            var funcPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Margin = new Padding(0),
                Height = 30
            };
            
            var label = new Label {Text = @"Функция: y =", Margin = new Padding(0, 5, 0, 0), Width = 75};
            var labelXmax = new Label {Text = @"Введите верхний предел функции:", Width = panel.Width};
            var labelXmin = new Label {Text = @"Введите нижний предел функции:", Width = panel.Width, Margin = new Padding(0, 10, 0, 0)};
            var labelStep = new Label {Text = @"Введите шаг сетки:", Width = panel.Width, Margin = new Padding(0, 10, 0, 0)};
            
            funcTextBox = new TextBox {Text = @"x"};
            var textBoxXmax = new TextBox {Text = @"1", Width = panel.Width};
            var textBoxXmin = new TextBox {Text = @"-1", Width = panel.Width};
            var textBoxStep = new TextBox {Text = @"100", Width = panel.Width};

            var button = new Button {Text = @"Построить", Margin = new Padding(0, 10, 0, 0)};
            button.Click += (sender, args) =>
            {
                int step, xMax, xMin;
                if (!(int.TryParse(textBoxStep.Text, out step) && 
                    int.TryParse(textBoxXmax.Text, out xMax) &&
                    int.TryParse(textBoxXmin.Text, out xMin)))
                {
                    MessageBox.Show(this, @"Введено не число");
                    return;
                }
                
                var form = new Chart(step, xMax, xMin, getFunc);
                form.ShowDialog(this);
            };
            
            funcPanel.Controls.Add(label);
            funcPanel.Controls.Add(funcTextBox);
            
            panel.Controls.Add(funcPanel);
            
            panel.Controls.Add(labelXmax);
            panel.Controls.Add(textBoxXmax);
            
            panel.Controls.Add(labelXmin);
            panel.Controls.Add(textBoxXmin);
         
            panel.Controls.Add(labelStep);
            panel.Controls.Add(textBoxStep);
            
            panel.Controls.Add(button);
            Controls.Add(panel);
        }

        public double getFunc(double x)
        {
            DataTable table = new DataTable();
            var func = funcTextBox.Text;
            var replace = func.Replace("x", $"{x.ToString(CultureInfo.InvariantCulture)}");

            var a = table.Compute(replace, "").ToString();
            return double.Parse(a);
        }

        public StartForm()
        {
            InitializeComponent();
        }
    }
}