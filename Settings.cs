using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spin_The_Wheel
{
    public partial class Settings : Form
    {
        Main MainForm;

        public Settings(Main f)
        {
            InitializeComponent();
            MainForm = f;
        }

        List<double> newValues = new List<double>();

        private void button1_Click(object sender, EventArgs e)
        {
            if (numChoice1.Value == 0 && numChoice2.Value == 0 &&
                numChoice3.Value == 0 && numChoice4.Value == 0 &&
                numChoice5.Value == 0 && numChoice6.Value == 0 &&
                numChoice7.Value == 0 && numChoice8.Value == 0)
            {
                MessageBox.Show("All values cannot be 0", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            newValues.Add(Decimal.ToDouble(numChoice1.Value));
            newValues.Add(Decimal.ToDouble(numChoice2.Value));
            newValues.Add(Decimal.ToDouble(numChoice3.Value));
            newValues.Add(Decimal.ToDouble(numChoice4.Value));
            newValues.Add(Decimal.ToDouble(numChoice5.Value));
            newValues.Add(Decimal.ToDouble(numChoice6.Value));
            newValues.Add(Decimal.ToDouble(numChoice7.Value));
            newValues.Add(Decimal.ToDouble(numChoice8.Value));

            MainForm.changeProb(newValues);

            MainForm.Refresh();
            this.Close();
        }
    }
}
