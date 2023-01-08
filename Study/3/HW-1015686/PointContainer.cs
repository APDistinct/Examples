using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NetPlan
{
    public class PointContainer : GroupBox
    {
        //public GroupBox newGroupBox;
        //public Button newButton;
        public CheckBox newCheckBox;
        public NumericUpDown newEdgeBeg;
        public NumericUpDown newEdgeEnd;
        public TextBox newEdgeWeight;        
        //public ComboBox newBox;
        //public Label newLabel;
        public int number;
        //public int isWrite;

        public PointContainer(int num)
        {
            number = num;
            newCheckBox = new CheckBox();
            newEdgeBeg = new NumericUpDown();
            newEdgeEnd = new NumericUpDown();
            newEdgeWeight = new TextBox();            
            
            newEdgeBeg.Parent = this;
            newEdgeEnd.Parent = this;
            newEdgeWeight.Parent = this;
            newCheckBox.Parent = this;
            newEdgeWeight.KeyUp += EdgeWeightChanged;
            newEdgeWeight.Text = "0";

            SetSize();
        }
        public PointContainer() : this(-1)
        {
        }
        void SetSize()
        {
            int width = 230;
            int height = 35;
            int spacing = 5;
            int curr = 0;
            int wwidth = 40;
            Size = new Size(width, height);
            
            newCheckBox.Location = new Point(30, 10);
            newCheckBox.Checked = true;
            newCheckBox.Size = new Size(wwidth, 20);
            curr += newCheckBox.Width;
            
            newEdgeBeg.Size = new Size(wwidth, height);
            newEdgeBeg.Location = new Point(spacing + curr, 10);
            curr += spacing + newEdgeBeg.Width;

            newEdgeEnd.Size = new Size(wwidth, height);
            newEdgeEnd.Location = new Point(spacing + curr, 10);
            curr += spacing + newEdgeEnd.Width;

            newEdgeWeight.Size = new Size(2*wwidth, height);
            newEdgeWeight.Location = new Point(spacing + curr, 10);
            curr += spacing + newEdgeWeight.Width;            
        }
        private void EdgeWeightChanged(object sender, EventArgs e)
        {
            double val = 0;
            string str = ((TextBox)sender).Text;
            if (!Double.TryParse(str, out val))
            {
                MessageBox.Show($"Недопустимое значение числа {str}");
                ((TextBox)sender).Text = str.Substring(0, str.Length - 1);
            }
            else
            {
                if (val == 0)
                {
                    MessageBox.Show($"Значение должно быть больше 0 ");
                }
            }
        }
    }
}
