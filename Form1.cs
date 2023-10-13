using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Text;
using System.IO;
using Newtonsoft.Json;

namespace Предсказываю_будущее
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "Ultimate prediction";
        private readonly string PREDICTION_CONFIG_PATH = $"{Environment.CurrentDirectory}\\predictionsConfig.json"; //путь к файлу json
        private string[] _predictions;
        private Random _random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private async void bPredict_Click(object sender, EventArgs e)
        {
            bPredict.Enabled = false;
            //рассинхрон
            await Task.Run(() =>
            {
                //Заполнение пол загрузки
                for (int i = 1; i < 100; i++) //итератор
                {
                    this.Invoke(new Action(()=>
                    {
                        UpdateProgressBar(i);
                        this.Text = $"{i}%"; //проценты на поле
                    }));
                    
                    Thread.Sleep(10); //блокировка юзер интерфейса

                }
            });
            var index = _random.Next(_predictions.Length);
            var prediction = _predictions[index];

            MessageBox.Show($"{prediction}!");
            progressBar1.Value = 0; //сброс процентов
            this.Text = APP_NAME; //начальное имя
            bPredict.Enabled = true;
        }
        private void UpdateProgressBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else
            {
                progressBar1.Value = i + 1;
            }
            progressBar1.Value = i;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = APP_NAME;
            try
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_PATH);
                _predictions = JsonConvert.DeserializeObject <string[]> (data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_predictions == null)
                {
                    Close();
                }
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("Предсказания закончились, кина не будет");
                    Close();
                }
            }
        }
    }
}
