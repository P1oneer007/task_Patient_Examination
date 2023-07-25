using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [Serializable]
    public class PaidPatient : Patient
    {
        [XmlElement("PaymentAmount")]
        public decimal PaymentAmount { get; set; }

        public PaidPatient()
        {
        }

        public PaidPatient(int id, string firstName, string lastName, DateTime dateOfBirth, string address, string phoneNumber, decimal paymentAmount)
            : base(id, firstName, lastName, dateOfBirth, address, phoneNumber)
        {
            PaymentAmount = paymentAmount;
        }

        public void PrintPaymentInfo()
        {
            Console.WriteLine($"Сумма оплаты: {PaymentAmount}");
        }
    }
}
