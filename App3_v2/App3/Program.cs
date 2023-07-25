using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Net;
using System.Globalization;

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
        /*   Console.Write(" ├");
             Console.WriteLine($"Идентификатор: {Id}");
             Console.Write(" ├─");
             Console.WriteLine($"Имя: {FirstName}");
             Console.Write(" ├─");
             Console.WriteLine($"Фамилия: {LastName}"); */
       // Console.WriteLine($"Id   Имя   Фамилия ");
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
// Базовый класс "Обследование"
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

// Класс "Лабораторное обследование"
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

// Класс "Рентгеновское обследование"
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

// Класс "Рентгеновский снимок"
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
        //Console.WriteLine($"Идентификатор: {Id}");
        Console.WriteLine($"Дата: {Date.ToShortDateString()}");
        Console.WriteLine($"Путь к файлу: {ImagePath}");
    }
}

// Класс "Платный пациент"
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

// Использование классов
class Program
{
    static void PrintPatientList(List<Patient> patients)
    {
        foreach (Patient patient in patients)
        {
            patient.MinPrintInfo();
        }
    }

    static void PrintPatientList1(List<Patient> patients)
    {
        foreach (Patient patient in patients)
        {
            patient.PrintInfo();
            patient.PrintExaminations();
        }
    }
    static void SearchByLastName(List<Patient> patients, string lastName)
    {
        List<Patient> searchResults = patients.FindAll(patient => patient.LastName.Equals(lastName));
        if (searchResults.Count > 0)
        {
            Console.WriteLine($"Результаты поиска по фамилии '{lastName}':");
            PrintPatientList1(searchResults);
        }
        else
        {
            Console.WriteLine($"Пациент с фамилией '{lastName}' не найден.");
        }
    }
    static void PrintPatients(List<Patient> patients)
    {
        foreach (Patient patient in patients)
        {
            patient.PrintInfo();
            patient.PrintExaminations();
            Console.WriteLine();
        }
    }
    static void Main(string[] args)
    {
        WindowUtility.FixeConsoleWindow(Configuration.ConsoleHeight, Configuration.ConsoleWidth);
        Console.Title = "Patient Examination";

        // Загрузка данных из XML файла 
        List<Patient> loadedPatients = LoadFromXml("patients.xml");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Список пациентов:");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Id     Имя          Фамилия ");
        Console.ForegroundColor = ConsoleColor.White;
        PrintPatientList(loadedPatients);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.ForegroundColor = ConsoleColor.White;


        List<Patient> patients = new List<Patient>();

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить пациента");
            // Console.WriteLine("2. Вывести список пациентов");
            Console.WriteLine("2. Поиск пациентов по Фамилии");
            Console.WriteLine("3. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    List<Patient> newPatients = LoadFromXml("patients.xml");
                    Console.Write("Идентификатор: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Имя: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Фамилия: ");
                    string lastName = Console.ReadLine();
                    Console.Write("Дата рождения (гггг-мм-дд): ");
                    DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());
                    Console.Write("Адрес: ");
                    string address = Console.ReadLine();
                    Console.Write("Телефон: ");
                    string phoneNumber = Console.ReadLine();
                    Patient patient = new Patient(id, firstName, lastName, dateOfBirth, address, phoneNumber);
                    Console.WriteLine("Введите данные обследования:");
                    Console.Write("Идентификатор: ");
                    int examinationId = int.Parse(Console.ReadLine());
                    Console.Write("Дата (гггг-мм-дд): ");
                    DateTime examinationDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Результаты: ");
                    string examinationResults = Console.ReadLine();
                    Examination examination = new Examination(examinationId, examinationDate, examinationResults);

                    patients.Add(patient);
                    patient.Examinations.Add(examination);
                    loadedPatients.AddRange(patients);
                    // Сохранение обновленного списка пациентов в файл
                    SaveToXml("patients.xml", loadedPatients);
                    Console.WriteLine("Пациент и его обследования успешно добавлены.");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case "2":
                    Console.WriteLine("Введите фамилию для поиска:");
                    string searchLastName = Console.ReadLine();
                    SearchByLastName(loadedPatients, searchLastName);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
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

            if (File.Exists(fileName))
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    return (List<Patient>)serializer.Deserialize(fileStream);
                }
            }
            else
            {
                return new List<Patient>();
            }
        }
    }

    class Configuration
    {
        public static int ElementsOnPage
        {
            get
            {
                int res;
                try
                {
                    res = Convert.ToInt32(ReadSetting("ElementsOnPage"));
                }
                catch (Exception)
                {
                    res = 20;
                    return res;
                }
                if (res <= 0) res = 20;
                return res;
            }
        }

        public static void SetElementsOnPage(int value)
        {
            AddUpdateAppSettings("ElementsOnPage", value.ToString());
        }

        public static int ConsoleHeight
        {
            get
            {
                int resHeight = MainPanelHeight + InfoPanelHeight + ComandPanelHeight;
                return resHeight;
            }
        }

        public const int ConsoleWidth = 70;//

        public static int MainPanelHeight
        {
            get { return ElementsOnPage + 2; }
        }

        public const int InfoPanelHeight = 3;//размер информационной панели

        public const int ComandPanelHeight = 3;

        public static int MessagesPosition
        {
            get { return Configuration.MainPanelHeight + Configuration.InfoPanelHeight + 1; }
        }

        public static int CommandPosition
        {
            get { return Configuration.MainPanelHeight + Configuration.InfoPanelHeight; }
        }


        private static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {

            }
        }
    }

    static class WindowUtility
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static void FixeConsoleWindow(int windowHeight, int windowWidth)
        {
            try
            {
                Console.WindowHeight = windowHeight;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WindowHeight = Console.LargestWindowHeight - 4;
                Configuration.SetElementsOnPage((Console.WindowHeight - Configuration.InfoPanelHeight - Configuration.ComandPanelHeight) - 4);
            }

            try
            {
                Console.WindowWidth = windowWidth;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WindowWidth = Console.LargestWindowWidth;
            }

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }
    }
}