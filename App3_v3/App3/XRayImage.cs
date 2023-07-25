using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [Serializable]
    public class XRayImage
    {
        public int Id { get; set; }
        [XmlElement("Date")]
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }

        public XRayImage()
        {
        }

        public XRayImage(int id, DateTime date, string imagePath)
        {
            Id = id;
            Date = date;
            ImagePath = imagePath;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Дата: {Date.ToShortDateString()}");
            Console.WriteLine($"Путь к файлу: {ImagePath}");
        }
    }

}
