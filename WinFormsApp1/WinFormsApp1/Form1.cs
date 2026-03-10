namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            woodCoefTxtBox.Enabled = false;
            temperatureTxtBox.Enabled = false;
            StoveKKDTxtBox.Enabled = false;

            ModeComboBox.Items.Add("Дрова");
            ModeComboBox.Items.Add("Брикети");
            ModeComboBox.SelectedIndex = 0;

            CalcComboBox.Items.Add("Намети");
            CalcComboBox.Items.Add("Об'єм");
            CalcComboBox.SelectedIndex = 0;

            USTlabel.Visible = true;
            USBLabel.Visible = true;
            USTCountTxtBox.Visible = true;
            USBCountTxtBox.Visible = true;
            SqureLabel.Visible = false;
            SqureTxtBox.Visible = false;
        }

        private void CalcBtn_Click(object sender, EventArgs e)
        {
            UpdateCounts();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (woodCoefCheckBox.Checked)
            {
                woodCoefTxtBox.Enabled = true;
            }
            else
            {
                woodCoefTxtBox.Enabled = false;
            }
        }

        private void temperatureCheckBoc_CheckedChanged(object sender, EventArgs e)
        {
            if (temperatureCheckBoc.Checked)
            {
                temperatureTxtBox.Enabled = true;
            }
            else
            {
                temperatureTxtBox.Enabled = false;
            }
        }

        private void StoveKKDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StoveKKDCheckBox.Checked)
            {
                StoveKKDTxtBox.Enabled = true;
            }
            else
            {
                StoveKKDTxtBox.Enabled = false;
            }
        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedComboBoxIndex = ModeComboBox.SelectedIndex;
        }

        private void CalcComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcComboBoxIndex = CalcComboBox.SelectedIndex;
            if (CalcComboBox.SelectedIndex == 0)
            {
                USTlabel.Visible = true;
                USBLabel.Visible = true;
                USTCountTxtBox.Visible = true;
                USBCountTxtBox.Visible = true;
                SqureLabel.Visible = false;
                SqureTxtBox.Visible = false;
            }
            else
            {
                USTlabel.Visible = false;
                USBLabel.Visible = false;
                USTCountTxtBox.Visible = false;
                USBCountTxtBox.Visible = false;
                SqureLabel.Visible = true;
                SqureTxtBox.Visible = true;
            }
        }
    }
}
