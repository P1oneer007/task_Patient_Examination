using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [Serializable]
    public class XRayExamination : Examination
    {
        [XmlElement("XRayImage")]
        public List<XRayImage> XRayImages { get; set; }

        public XRayExamination()
        {
            XRayImages = new List<XRayImage>();
        }

        public XRayExamination(int id, DateTime date, string results)
            : base(id, date, results)
        {
            XRayImages = new List<XRayImage>();
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine("Рентгеновские снимки:");
            foreach (XRayImage xrayImage in XRayImages)
            {
                xrayImage.PrintInfo();
            }
        }
    }
}
