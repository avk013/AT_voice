using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AT_voice
{
    /*  public class Settings
      {
          private string _serialPort;


          public string sserialPort
          {
              get { return _serialPort; }
              set { _serialPort = value; }
          }
      }*/

    public partial class Form1 : Form
    {
        #region Settings action
        XMLconf xmlconf = new XMLconf();
        //Props props = new Props(); //экземпляр класса с настройками 
        //Запись настроек
        private void writeSetting()
        {
            //Запись значения в ComboBox1
            //props.Fields.TextValue = ComboBox1.Text;
            //Запись значения в checkBox1
            //  props.Fields.BoolValue = checkBox1.Checked;
          //  xmlconf.Fields.ComboBoxValue = comboBox1;
            xmlconf.WriteXml();
          
        }
        //Чтение настроек
        private void readSetting()
        {
            xmlconf.ReadXml();
           // ComboBox1.Text = props.Fields.TextValue;
           // checkBox1.Checked = props.Fields.BoolValue;
        }
        #endregion

        SerialPort _serialPort;
        int flag1=0;

        //    Settings settings = null;
        #region Form Action
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
            timer1.Interval=10000;
            timer1.Enabled = false;
            readSetting();




            /*   if (File.Exists("settings.xml"))
                {
                    XmlSerializer nser = new XmlSerializer(typeof(Settings));
                    XmlSerializer deser = new XmlSerializer(typeof(Settings));
                    TextReader reader = new StreamReader("settings.xml");
                    settings = (Settings)nser.Deserialize(reader);
                    reader.Close();

                }
                else
                {
                    settings = new Settings();

                   // Console.WriteLine("_serialPort");
                   // string ismtpHost = Console.ReadLine();
                   // settings.serialPort = _serialPort;
                  //  settings.sserialPort= _serialPort;

                    XmlSerializer ser = new XmlSerializer(typeof(Settings));
                    TextWriter writer = new StreamWriter("settings.xml");
                    ser.Serialize(writer, settings);
                    writer.Close();
                }*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(comboBox1.SelectedItem.ToString());

            _serialPort = new SerialPort(comboBox1.SelectedItem.ToString(),
                                        9600, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new
       SerialDataReceivedEventHandler(port_DataReceived);

            // Begin communications
            _serialPort.Open();
            _serialPort.WriteLine("AT"); //ath повесить трубку
            _serialPort.WriteLine("AT+DDET=1"); //включаем режим распознавания DTMF
        }
        private void port_DataReceived(object sender,
      SerialDataReceivedEventArgs e)
        {
            string serout="";
            // Show all the incoming data in the port's buffer
            // Console.WriteLine(_serialPort.ReadExisting());
            //textBox1.Text += _serialPort.ReadExisting();
            textBox1.Invoke(
               (ThreadStart)delegate ()
               {
               serout= _serialPort.ReadExisting();
                   textBox1.Text+=serout+Environment.NewLine;
                   // "+DTMF:"
                   if (serout.Contains("RING"))
                   {
                       string nom = serout.Replace(Environment.NewLine, "");
                       string dnom = "+CLIP: \"";
                       // int nom2 = nom.IndexOf("CLIP:");
                       nom = nom.Substring(nom.IndexOf("+CLIP:") + dnom.Length, 13);
                       // nom = nom.Substring(nom.IndexOf("+CLIP:")+3,13);
                       //MessageBox.Show(nom);
                       textBox3.Text = nom;
                       flag1++;  //кк узнать что поднята трубка?
                       timer1.Enabled = true;
                      // if (flag1 >= 2) { flag1 = 0; _serialPort.WriteLine("AT+CREC=4,\"C:\\User\\1.amr\",0,100"); }
                   }

               });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //_serialPort.Write(textBox2.Text);
            if(_serialPort.IsOpen)
            _serialPort.WriteLine(textBox2.Text);
            textBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nom = "+CLIP: \" +380966413593",dnom = "+CLIP: \"";
           // int nom2 = nom.IndexOf("CLIP:");
           nom = nom.Substring(nom.IndexOf("+CLIP:") + dnom.Length, 14);
            MessageBox.Show(nom);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {//приветствуем
            //_serialPort.WriteLine("Ati");
            
            //if (flag1 >= 2)
            { flag1 = 0; 
                _serialPort.WriteLine("AT+CREC=4,\"C:\\User\\hello.amr\",0,100;+CREC=4,\"C:\\User\\press.amr\",0,100");
                //_serialPort.WriteLine("AT+CREC=4,\"C:\\User\\press.amr\",0,100");
            }
            timer1.Enabled = false;
        }
        private string at_command(string at_com)
        { _serialPort.WriteLine(at_com);
           //string res= _serialPort.ReadExisting();
            return "";
        }
        private string at_response()
        {           
            string res = _serialPort.ReadExisting();
            return res;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(at_command("ATI"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            writeSetting();
        }
    }
    #endregion


}
