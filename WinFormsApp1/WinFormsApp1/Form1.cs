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
        }

        private void CalcBtn_Click(object sender, EventArgs e)
        {
            int i = 1;
            int j = 1;
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
    }
}
