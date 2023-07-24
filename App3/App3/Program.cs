using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
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

    public void MinPrintInfo()
    {
        Console.Write(" ├");
        Console.WriteLine($"Идентификатор: {Id}");
        Console.Write(" ├─");
        Console.WriteLine($"Имя: {FirstName}");
        Console.Write(" ├─");
        Console.WriteLine($"Фамилия: {LastName}");
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
        //
        //
        //
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

        // Сохранение данных в XML файл
        SaveToXml("patients.xml", new List<Patient> { patient1, patient2 });
        // Загрузка данных из XML файла
        List<Patient> loadedPatients = LoadFromXml("patients.xml");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Список пациентов:");
        PrintPatientList(loadedPatients);

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Введите фамилию для поиска:");
        string searchLastName = Console.ReadLine();
        SearchByLastName(loadedPatients, searchLastName);
        do
            {

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Введите фамилию для поиска (exit для выхода):");
                searchLastName = Console.ReadLine();
                if (searchLastName.ToLower() == "exit")
                {
                    break;
                }

                List<Patient> searchResults = loadedPatients.Where(p => p.LastName.ToLower() == searchLastName.ToLower()).ToList();

                if (searchResults.Count == 0)
                {
                    Console.WriteLine("Пациент не найден.");
                }
                else
                {
                Console.WriteLine("Результаты поиска:");
                    PrintPatients(searchResults);
                }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();

            } while (true);
}

       // PrintPatientData(loadedPatients);

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

    static class Configuration
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