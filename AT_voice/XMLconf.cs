using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AT_voice
{//Класс определяющий какие настройки есть в программе
    public class XMLconfields
    {
        public String XMLFileName = Environment.CurrentDirectory + "\\settings.xml";
        //Чтобы добавить настройку в программу просто добавьте туда строку вида -
        //public ТИП ИМЯ_ПЕРЕМЕННОЙ = значение_переменной_по_умолчанию;
        public String TextValue = @"File Settings";
        public DateTime DateValue = new DateTime(2011, 1, 1);
        public Decimal DecimalValue = 555;
        public Boolean BoolValue = true;
       // public ComboBox ComboBoxValue;
    }
    class XMLconf
    {
        public XMLconfields Fields;

        public XMLconf()
        {
            Fields = new XMLconfields();
        }
        //Запись настроек в файл
        public void WriteXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(XMLconfields));

            TextWriter writer = new StreamWriter(Fields.XMLFileName);
            ser.Serialize(writer, Fields);
            writer.Close();
        }
        //Чтение насроек из файла
        public void ReadXml()
        {
            if (File.Exists(Fields.XMLFileName))
            {
                XmlSerializer ser = new XmlSerializer(typeof(XMLconfields));
                TextReader reader = new StreamReader(Fields.XMLFileName);
                Fields = ser.Deserialize(reader) as XMLconfields;
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }
        }
    }
}
