using NLog;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class CurrentWeather
    {
        public float Temperature { get; set; }
        public float WindSpeed { get; set; }
        public string Time { get; set; }
    }

    public class OpenMeteoResponse
    {
        public CurrentWeather Current_Weather { get; set; }
    }

    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private int USTCount = 4;
        private int USBCount = 6;
        private int temperatureInside = 18;
        private int woodCoef = 500;
        private float stoveKKD = 0.8f;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private DateTime startDate;
        private DateTime dewDate;
        private int SelectedComboBoxIndex = 0;

        Dictionary<int, Dictionary<int, float>> USTConsumptionTable = new Dictionary<int, Dictionary<int, float>>()
        {
            { 0,new Dictionary<int, float>{ {15,0.96f }, { 10, 2.55f }, { 5, 4.18f }, { 0, 5.77f }, { -5, 7.38f }, { -10, 8.99f }, { -15, 10.60f }, { -20, 12.21f }, { -25, 13.81f }, { -30, 15.41f }, { -35, 17.03f } } },
            { 2,new Dictionary<int, float>{ {15,0.98f }, { 10, 2.62f }, { 5, 4.32f }, { 0, 5.98f }, { -5, 7.66f }, { -10, 9.38f }, { -15, 11.10f }, { -20, 12.83f }, { -25, 14.56f }, { -30, 16.32f }, { -35, 18.12f } } },
            { 4,new Dictionary<int, float>{ {15,1.03f }, { 10, 2.78f }, { 5, 4.58f }, { 0, 6.34f }, { -5, 8.13f }, { -10, 9.95f }, { -15, 11.79f }, { -20, 13.64f }, { -25, 15.51f }, { -30, 17.40f }, { -35, 19.32f } } },
            { 6,new Dictionary<int, float>{ {15,1.12f }, { 10, 3.03f }, { 5, 4.99f }, { 0, 6.92f }, { -5, 8.90f }, { -10, 10.75f }, { -15, 12.94f }, { -20, 15.00f }, { -25, 17.07f }, { -30, 19.18f }, { -35, 21.32f } } },
            { 8,new Dictionary<int, float>{ {15,1.26f }, { 10, 3.40f }, { 5, 5.60f }, { 0, 7.79f }, { -5, 10.03f }, { -10, 12.09f }, { -15, 14.62f }, { -20, 16.96f }, { -25, 19.34f }, { -30, 21.75f }, { -35, 24.24f } } },
            { 10,new Dictionary<int, float>{ {15,1.43f }, { 10, 3.84f }, { 5, 6.32f }, { 0, 8.80f }, { -5, 11.34f }, { -10, 13.93f }, { -15, 16.57f }, { -20, 19.26f }, { -25, 22.00f }, { -30, 24.78f }, { -35, 27.66f } } },
            { 12,new Dictionary<int, float>{ {15,1.64f }, { 10, 4.40f }, { 5, 7.24f }, { 0, 10.11f }, { -5, 13.04f }, { -10, 15.73f }, { -15, 19.12f }, { -20, 22.24f }, { -25, 25.43f }, { -30, 28.71f }, { -35, 32.07f } } },
            { 14,new Dictionary<int, float>{ {15,1.89f }, { 10, 5.09f }, { 5, 8.38f }, { 0, 11.72f }, { -5, 15.13f }, { -10, 18.32f }, { -15, 22.23f }, { -20, 25.89f }, { -25, 29.66f }, { -30, 33.52f }, { -35, 37.48f } } }
        };

        Dictionary<int, Dictionary<int, float>> USBConsumptionTable = new Dictionary<int, Dictionary<int, float>>()
        {
            { 0,new Dictionary<int, float>{ {15,1.93f }, { 10, 5.12f }, { 5, 8.39f }, { 0, 11.58f }, { -5, 14.81f }, { -10, 18.04f }, { -15, 21.27f }, { -20, 24.50f }, { -25, 27.72f }, { -30, 30.93f }, { -35, 34.18f } } },
            { 2,new Dictionary<int, float>{ {15,1.97f }, { 10, 5.26f }, { 5, 8.67f }, { 0, 12.00f }, { -5, 15.37f }, { -10, 18.82f }, { -15, 22.28f }, { -20, 25.75f }, { -25, 29.22f }, { -30, 32.75f }, { -35, 36.36f } } },
            { 4,new Dictionary<int, float>{ {15,2.07f }, { 10, 5.58f }, { 5, 9.19f }, { 0, 12.72f }, { -5, 16.32f }, { -10, 19.97f }, { -15, 23.66f }, { -20, 27.37f }, { -25, 31.13f }, { -30, 34.92f }, { -35, 38.77f } } },
            { 6,new Dictionary<int, float>{ {15,2.25f }, { 10, 6.08f }, { 5, 10.01f }, { 0, 13.89f }, { -5, 17.86f }, { -10, 21.57f }, { -15, 25.97f }, { -20, 30.10f }, { -25, 34.26f }, { -30, 38.49f }, { -35, 42.79f } } },
            { 8,new Dictionary<int, float>{ {15,2.53f }, { 10, 6.82f }, { 5, 11.24f }, { 0, 15.63f }, { -5, 20.13f }, { -10, 24.26f }, { -15, 29.34f }, { -20, 34.04f }, { -25, 38.81f }, { -30, 43.65f }, { -35, 48.65f } } },
            { 10,new Dictionary<int, float>{ {15,2.87f }, { 10, 7.71f }, { 5, 12.68f }, { 0, 17.66f }, { -5, 22.76f }, { -10, 27.96f }, { -15, 33.25f }, { -20, 38.65f }, { -25, 44.15f }, { -30, 49.73f }, { -35, 55.51f } } },
            { 12,new Dictionary<int, float>{ {15,2.39f }, { 10, 8.83f }, { 5, 14.53f }, { 0, 20.29f }, { -5, 26.17f }, { -10, 31.57f }, { -15, 38.37f }, { -20, 44.63f }, { -25, 51.03f }, { -30, 57.62f }, { -35, 64.36f } } },
            { 14,new Dictionary<int, float>{ {15,3.79f }, { 10, 10.21f }, { 5, 16.82f }, { 0, 23.52f }, { -5, 30.36f }, { -10, 36.76f }, { -15, 44.61f }, { -20, 51.96f }, { -25, 59.52f }, { -30, 67.27f }, { -35, 75.22f } } }
        };


        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            temperatureTxtBox = new RichTextBox();
            startDatePicker = new DateTimePicker();
            dewDatePicker = new DateTimePicker();
            label1 = new Label();
            USTCountTxtBox = new RichTextBox();
            label2 = new Label();
            label3 = new Label();
            USBCountTxtBox = new RichTextBox();
            temperatureCheckBoc = new CheckBox();
            label4 = new Label();
            CalcBtn = new Button();
            outputTxtBox = new RichTextBox();
            woodCoefTxtBox = new RichTextBox();
            woodCoefCheckBox = new CheckBox();
            label5 = new Label();
            label6 = new Label();
            StoveKKDTxtBox = new RichTextBox();
            label7 = new Label();
            StoveKKDCheckBox = new CheckBox();
            ModeComboBox = new ComboBox();
            label8 = new Label();
            SuspendLayout();
            // 
            // temperatureTxtBox
            // 
            temperatureTxtBox.Location = new Point(46, 87);
            temperatureTxtBox.Name = "temperatureTxtBox";
            temperatureTxtBox.Size = new Size(131, 22);
            temperatureTxtBox.TabIndex = 0;
            temperatureTxtBox.Text = "18";
            // 
            // startDatePicker
            // 
            startDatePicker.Location = new Point(46, 128);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(200, 23);
            startDatePicker.TabIndex = 1;
            // 
            // dewDatePicker
            // 
            dewDatePicker.Location = new Point(310, 128);
            dewDatePicker.Name = "dewDatePicker";
            dewDatePicker.Size = new Size(200, 23);
            dewDatePicker.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 71);
            label1.Name = "label1";
            label1.Size = new Size(176, 15);
            label1.TabIndex = 4;
            label1.Text = "Средня температура всередені";
            // 
            // USTCountTxtBox
            // 
            USTCountTxtBox.Location = new Point(253, 38);
            USTCountTxtBox.Name = "USTCountTxtBox";
            USTCountTxtBox.Size = new Size(95, 20);
            USTCountTxtBox.TabIndex = 6;
            USTCountTxtBox.Text = "4";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(253, 20);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 7;
            label2.Text = "Кількість УСТ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(415, 20);
            label3.Name = "label3";
            label3.Size = new Size(81, 15);
            label3.TabIndex = 9;
            label3.Text = "Кількість УСБ";
            // 
            // USBCountTxtBox
            // 
            USBCountTxtBox.Location = new Point(415, 38);
            USBCountTxtBox.Name = "USBCountTxtBox";
            USBCountTxtBox.Size = new Size(95, 20);
            USBCountTxtBox.TabIndex = 8;
            USBCountTxtBox.Text = "6";
            // 
            // temperatureCheckBoc
            // 
            temperatureCheckBoc.AutoSize = true;
            temperatureCheckBoc.Location = new Point(25, 91);
            temperatureCheckBoc.Name = "temperatureCheckBoc";
            temperatureCheckBoc.Size = new Size(15, 14);
            temperatureCheckBoc.TabIndex = 10;
            temperatureCheckBoc.UseVisualStyleBackColor = true;
            temperatureCheckBoc.CheckedChanged += temperatureCheckBoc_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(257, 132);
            label4.Name = "label4";
            label4.Size = new Size(21, 15);
            label4.TabIndex = 11;
            label4.Text = "по";
            // 
            // CalcBtn
            // 
            CalcBtn.Location = new Point(415, 417);
            CalcBtn.Name = "CalcBtn";
            CalcBtn.Size = new Size(83, 23);
            CalcBtn.TabIndex = 12;
            CalcBtn.Text = "Розрахунок";
            CalcBtn.UseVisualStyleBackColor = true;
            CalcBtn.Click += CalcBtn_Click;
            // 
            // outputTxtBox
            // 
            outputTxtBox.Location = new Point(22, 169);
            outputTxtBox.Name = "outputTxtBox";
            outputTxtBox.Size = new Size(488, 242);
            outputTxtBox.TabIndex = 13;
            outputTxtBox.Text = "";
            // 
            // woodCoefTxtBox
            // 
            woodCoefTxtBox.Location = new Point(257, 87);
            woodCoefTxtBox.Name = "woodCoefTxtBox";
            woodCoefTxtBox.Size = new Size(91, 22);
            woodCoefTxtBox.TabIndex = 14;
            woodCoefTxtBox.Text = "500";
            // 
            // woodCoefCheckBox
            // 
            woodCoefCheckBox.AutoSize = true;
            woodCoefCheckBox.Location = new Point(236, 91);
            woodCoefCheckBox.Name = "woodCoefCheckBox";
            woodCoefCheckBox.Size = new Size(15, 14);
            woodCoefCheckBox.TabIndex = 15;
            woodCoefCheckBox.UseVisualStyleBackColor = true;
            woodCoefCheckBox.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(257, 71);
            label5.Name = "label5";
            label5.Size = new Size(97, 15);
            label5.TabIndex = 16;
            label5.Text = "Коофіціент дров";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(23, 134);
            label6.Name = "label6";
            label6.Size = new Size(12, 15);
            label6.TabIndex = 17;
            label6.Text = "з";
            // 
            // StoveKKDTxtBox
            // 
            StoveKKDTxtBox.Location = new Point(415, 87);
            StoveKKDTxtBox.Name = "StoveKKDTxtBox";
            StoveKKDTxtBox.Size = new Size(95, 22);
            StoveKKDTxtBox.TabIndex = 18;
            StoveKKDTxtBox.Text = "0.8";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(414, 71);
            label7.Name = "label7";
            label7.Size = new Size(74, 15);
            label7.TabIndex = 19;
            label7.Text = "К.К.Д. печей";
            // 
            // StoveKKDCheckBox
            // 
            StoveKKDCheckBox.AutoSize = true;
            StoveKKDCheckBox.Location = new Point(394, 91);
            StoveKKDCheckBox.Name = "StoveKKDCheckBox";
            StoveKKDCheckBox.Size = new Size(15, 14);
            StoveKKDCheckBox.TabIndex = 20;
            StoveKKDCheckBox.UseVisualStyleBackColor = true;
            StoveKKDCheckBox.CheckedChanged += StoveKKDCheckBox_CheckedChanged;
            // 
            // ModeComboBox
            // 
            ModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ModeComboBox.FormattingEnabled = true;
            ModeComboBox.Location = new Point(46, 35);
            ModeComboBox.Name = "ModeComboBox";
            ModeComboBox.Size = new Size(121, 23);
            ModeComboBox.TabIndex = 21;
            ModeComboBox.SelectedIndexChanged += ModeComboBox_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(46, 20);
            label8.Name = "label8";
            label8.Size = new Size(69, 15);
            label8.TabIndex = 22;
            label8.Text = "Тип палива";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(530, 460);
            Controls.Add(label8);
            Controls.Add(ModeComboBox);
            Controls.Add(StoveKKDCheckBox);
            Controls.Add(label7);
            Controls.Add(StoveKKDTxtBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(woodCoefCheckBox);
            Controls.Add(woodCoefTxtBox);
            Controls.Add(outputTxtBox);
            Controls.Add(CalcBtn);
            Controls.Add(label4);
            Controls.Add(temperatureCheckBoc);
            Controls.Add(label3);
            Controls.Add(USBCountTxtBox);
            Controls.Add(label2);
            Controls.Add(USTCountTxtBox);
            Controls.Add(label1);
            Controls.Add(dewDatePicker);
            Controls.Add(startDatePicker);
            Controls.Add(temperatureTxtBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Consumption";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox temperatureTxtBox;
        private DateTimePicker startDatePicker;
        private DateTimePicker dewDatePicker;
        private Label label1;
        private RichTextBox USTCountTxtBox;
        private Label label2;
        private Label label3;
        private RichTextBox USBCountTxtBox;
        private CheckBox temperatureCheckBoc;
        private Label label4;
        private Button CalcBtn;
        private RichTextBox outputTxtBox;
        private RichTextBox woodCoefTxtBox;
        private CheckBox woodCoefCheckBox;
        private Label label5;
        private Label label6;
        private RichTextBox StoveKKDTxtBox;
        private Label label7;
        private CheckBox StoveKKDCheckBox;
        private ComboBox ModeComboBox;
        private Label label8;


        static async Task<List<CurrentWeather>> GetWeather(DateTime start, DateTime end)
        {
            List<CurrentWeather> WeatherList = new List<CurrentWeather>();
            HttpClient client = new HttpClient();
            string url = "https://api.open-meteo.com/v1/forecast?latitude=49.44&longitude=32.05&daily=wind_speed_10m_mean,temperature_2m_mean&start_date="+ start.ToString("yyyy-MM-dd")+"&end_date="+ end.ToString("yyyy-MM-dd");

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            string responseTxt = await client.GetStringAsync(url);
            using JsonDocument doc = JsonDocument.Parse(responseTxt);
            JsonElement daily = doc.RootElement.GetProperty("daily");

            var dates = daily.GetProperty("time");
            var meanTemps = daily.GetProperty("temperature_2m_mean");
            var meanSpeed = daily.GetProperty("wind_speed_10m_mean");

           

            if (dates.GetArrayLength()>0)
            for (int i = 0; i < dates.GetArrayLength(); i++)
            {
                CurrentWeather tmp = new CurrentWeather();
                tmp.Time = dates[i].GetString();
                if (meanTemps[i].TryGetDouble(out double temperature))
                {
                        tmp.Temperature = (float)temperature;
                }
                if (meanSpeed[i].TryGetDouble(out double speed))
                {
                    tmp.WindSpeed = (float)speed;
                }
                    WeatherList.Add(tmp);
            }

            return WeatherList;

        }

        public async void UpdateCounts()
        {
            if (temperatureCheckBoc.Checked)
            if (!int.TryParse(temperatureTxtBox.Text, out temperatureInside))
            {
                Logger.Error("Failed to read temperature as int");
                outputTxtBox.Text = "Невдалось зчитати температуру як число";
            }

            if (woodCoefCheckBox.Checked)
            if (!int.TryParse(woodCoefTxtBox.Text, out woodCoef))
            {
                Logger.Error("Failed to read wood cooficient as int");
                outputTxtBox.Text = "Невдалось зчитати коефіціент дров як число";
            }

            if (StoveKKDCheckBox.Checked)
            if (!float.TryParse(StoveKKDTxtBox.Text, out stoveKKD))
            {
                Logger.Error("Failed to read stove KKD as float");
                outputTxtBox.Text = "Невдалось зчитати ККД печей як число або дріб";
            }

            if (!int.TryParse(USTCountTxtBox.Text, out USTCount))
            {
                Logger.Error("Failed to read USTCount as int");
                outputTxtBox.Text = "Невдалось зчитати кількість УСТ як число";
            }

            if (!int.TryParse(USBCountTxtBox.Text, out USBCount))
            {
                Logger.Error("Failed to read USBCount as int");
                outputTxtBox.Text = "Невдалось зчитати кількість УСБ як число";
            }

            startDate = startDatePicker.Value;
            dewDate = dewDatePicker.Value;


            string outputResult = "";
            List<CurrentWeather> result = await GetWeather(startDate, dewDate);
            for (int i = 0; i < result.Count; i++)
            {
                float USTWoodValue = 0.0f;
                float USBWoodValue = 0.0f;

                int diffWind = (int)result[i].WindSpeed / 2; diffWind *= 2;
                if (diffWind > 14) { diffWind = 14; }
                if (USTConsumptionTable.TryGetValue(diffWind, out Dictionary<int, float> localDictionaryUST))
                {                    
                    int diffTemp = (int)result[i].Temperature / 5; 
                    diffTemp *= 5 ;
                    if (localDictionaryUST.TryGetValue(diffTemp, out float woodValue))
                    {
                        float temperatureCoef = (temperatureInside - result[i].Temperature) / (18 - result[i].Temperature);
                        USTWoodValue = woodValue * temperatureCoef; 
                    }
                }

                if (USBConsumptionTable.TryGetValue(diffWind, out Dictionary<int, float> localDictionaryUSB))
                {                  
                    int diffTemp = (int)result[i].Temperature / 5;
                    diffTemp *= 5;
                    if (localDictionaryUSB.TryGetValue(diffTemp, out float woodValue))
                    {
                        float temperatureCoef = (temperatureInside - result[i].Temperature) / (18 - result[i].Temperature);
                        USBWoodValue = woodValue * temperatureCoef;
                    }
                }

                float consumptionValue = ((USTWoodValue * 2 * USTCount) + (USBWoodValue * 2 * USBCount)) / stoveKKD;
                string tmpResult = result[i].Time + "  Вітер = " + result[i].WindSpeed.ToString() + " Температура = " + result[i].Temperature.ToString();
               

                if (SelectedComboBoxIndex == 0)
                {
                    consumptionValue = consumptionValue / 0.266f / woodCoef;
                    tmpResult += " Розхід = " + consumptionValue.ToString() + " м3 дров \n";
                }
                else 
                {
                    consumptionValue /= 0.578f;
                    tmpResult += " Розхід = " + consumptionValue.ToString() + " кг брикетів \n";
                }

                outputResult += tmpResult;
            }

            
            outputTxtBox.Text = outputResult;

        }

        
    }
}
