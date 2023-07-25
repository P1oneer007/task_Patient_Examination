using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [Serializable]

    public class LaboratoryExamination : Examination
    {
        [XmlElement("LabName")]
        public string LabName { get; set; }

        public LaboratoryExamination()
        {
        }
        public LaboratoryExamination(int id, DateTime date, string results, string labName)
            : base(id, date, results)
        {
            LabName = labName;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Лаборатория: {LabName}");
        }
    }
}
