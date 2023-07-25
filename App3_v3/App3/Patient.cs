using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App3
{
    [XmlInclude(typeof(LaboratoryExamination))]
    [XmlInclude(typeof(PaidPatient))]
    public class Patient
    {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            [XmlElement("Examination")]
            public List<Examination> Examinations { get; set; }
            public Patient()
            {
                Examinations = new List<Examination>();
            }
            public Patient(int id, string firstName, string lastName, DateTime dateOfBirth, string address, string phoneNumber)
            {
                Id = id;
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                Address = address;
                PhoneNumber = phoneNumber;
                Examinations = new List<Examination>();
            }
            public Patient(string? firstName, string? lastName, DateTime dateOfBirth, string? address, string? phoneNumber)
            {
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                Address = address;
                PhoneNumber = phoneNumber;
            }
            public void AddExamination(int id, DateTime date, string results)
            {
                Examination examination = new Examination(id, date, results);
                Examinations.Add(examination);
            }
            public void MinPrintInfo()
            {
                Console.WriteLine($"{Id}      {FirstName}         {LastName}");
            }
            public void PrintInfo()
            {
                Console.WriteLine($"Идентификатор: {Id}");
                Console.WriteLine($"Имя: {FirstName}");
                Console.WriteLine($"Фамилия: {LastName}");
                Console.WriteLine($"Дата рождения: {DateOfBirth.ToShortDateString()}");
                Console.WriteLine($"Адрес: {Address}");
                Console.WriteLine($"Телефон: {PhoneNumber}");
            }
            public void PrintExaminations()
            {
                Console.WriteLine("Обследования:");
                foreach (Examination examination in Examinations)
                {
                    examination.PrintInfo();
                }
            }
    }
}
