using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [XmlInclude(typeof(LaboratoryExamination))]
    [XmlInclude(typeof(XRayExamination))]
    public class Examination
    {

            public int Id { get; set; }
            [XmlElement("Date")]
            public DateTime Date { get; set; }
            public string Results { get; set; }
            public Examination()
            {
            }
            public Examination(int id, DateTime date, string results)
            {
                Id = id;
                Date = date;
                Results = results;
            }
            public virtual void PrintInfo()
            {
                // Console.WriteLine($"Идентификатор: {Id}");
                Console.WriteLine($"Дата: {Date.ToShortDateString()}");
                Console.WriteLine($"Результаты: {Results}");
            }
    }
}
