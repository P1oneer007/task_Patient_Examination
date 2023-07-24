using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

// Класс "Пациент"
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

    public void PrintInfo()
    {
        Console.WriteLine($"Идентификатор: {Id}");
        Console.WriteLine($"Имя: {FirstName}");
        Console.WriteLine($"Фамилия: {LastName}");
        Console.WriteLine($"Дата рождения: {DateOfBirth.ToShortDateString()}");
        Console.WriteLine($"Адрес: {Address}");
        Console.WriteLine($"Телефон: {PhoneNumber}");
    }

    public List<Patient> SearchByLastName(string lastName)
    {
        List<Patient> patients = new List<Patient>();
        foreach (Patient patient in patients)
        {
            if (patient.LastName == lastName.Trim())
            {
                patients.Add(patient);
            }
        }
        return patients;
    }
}

// Базовый класс "Обследование"
[XmlInclude(typeof(LaboratoryExamination))]
[XmlInclude(typeof(XRayExamination))]
public class Examination
{
    public int Id { get; set; }
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
        Console.WriteLine($"Идентификатор: {Id}");
        Console.WriteLine($"Дата: {Date.ToShortDateString()}");
        Console.WriteLine($"Результаты: {Results}");
    }

    public List<Examination> SearchByDate(DateTime date)
    {
        List<Examination> examinations = new List<Examination>();
        foreach (Examination examination in examinations)
        {
            if (examination.Date == date)
            {
                examinations.Add(examination);
            }
        }
        return examinations;
    }
}

// Класс "Лабораторное обследование"
[Serializable]
public class LaboratoryExamination : Examination
{
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

// Класс "Рентгеновское обследование"
[Serializable]
public class XRayExamination : Examination
{
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

// Класс "Рентгеновский снимок"
[Serializable]
public class XRayImage
{
    public int Id { get; set; }
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
        Console.WriteLine($"Идентификатор: {Id}");
        Console.WriteLine($"Дата: {Date.ToShortDateString()}");
        Console.WriteLine($"Путь к файлу: {ImagePath}");
    }
}

// Класс "Платный пациент"
[Serializable]
public class PaidPatient : Patient
{
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

// Использование классов
class Program
{
    static void Main(string[] args)
    {
        // Создание объектов пациентов
        Patient patient1 = new Patient(1, "Иван", "Иванов", new DateTime(1995, 5, 10), "ул. Ленина, 1", "123456789");
        PaidPatient patient2 = new PaidPatient(2, "Петр", "Петров", new DateTime(1985, 8, 15), "ул. Пушкина, 2", "987654321", 1000);

        // Создание объектов обследований
        LaboratoryExamination labExamination1 = new LaboratoryExamination(1, new DateTime(2021, 10, 5), "Нормальные результаты", "Лаборатория 1");
        XRayExamination xrayExamination1 = new XRayExamination(2, new DateTime(2021, 11, 15), "Выявлены отклонения");

        // Создание объектов рентгеновских снимков
        XRayImage xrayImage1 = new XRayImage(1, new DateTime(2021, 10, 5), "C:\\Images\\xray1.jpg");
        XRayImage xrayImage2 = new XRayImage(2, new DateTime(2021, 10, 5), "C:\\Images\\xray2.jpg");

        // Связывание пациентов с обследованиями
        patient1.Examinations.Add(labExamination1);
        patient2.Examinations.Add(xrayExamination1);

        // Связывание обследований с рентгеновскими снимками
        xrayExamination1.XRayImages.Add(xrayImage1);
        xrayExamination1.XRayImages.Add(xrayImage2);

        // Печать информации о пациентах, обследованиях и рентгеновских снимках
        patient1.PrintInfo();
        Console.WriteLine();
        labExamination1.PrintInfo();
        Console.WriteLine();
        patient2.PrintInfo();
        Console.WriteLine();
        xrayExamination1.PrintInfo();

        // Сохранение данных в XML файл
        SaveToXml("patients.xml", new List<Patient> { patient1, patient2 });
       // SaveToXml("patients.xml", patients);
        // Загрузка данных из XML файла
        List<Patient> loadedPatients = LoadFromXml("patients.xml");

        // Печать загруженных данных
        Console.WriteLine("\nЗагруженные данные:");
        foreach (Patient patient in loadedPatients)
        {
            patient.PrintInfo();
            Console.WriteLine();
        }
        static void SaveToXml(string fileName, List<Patient> patients)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Patient>));
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, patients);
            }
        }
    }

    // Метод для сохранения данных в XML файл
    static void SaveToXml(string fileName, List<Patient> patients)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Patient>));
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        {
            serializer.Serialize(fileStream, patients);
        }
    }

    // Метод для загрузки данных из XML файла
    static List<Patient> LoadFromXml(string fileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Patient>));
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        {
            return (List<Patient>)serializer.Deserialize(fileStream);
        }
    }
}