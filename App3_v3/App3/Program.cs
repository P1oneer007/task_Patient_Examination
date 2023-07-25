using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Net;
using System.Globalization;
using App3;
using Configuration = App3.Configuration;

// Класс "Пациент"
// Базовый класс "Обследование"
// Класс "Лабораторное обследование"
// Класс "Рентгеновское обследование"
// Класс "Рентгеновский снимок"
// Класс "Платный пациент"
// Использование классов
class Program
{
   static void PrintPatientList(List<Patient> patients)
    {
        foreach ( Patient patient in patients)
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
            Console.WriteLine("2. Поиск пациентов по Фамилии");
            Console.WriteLine("3. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    List<Patient> newPatients = LoadFromXml("patients.xml");

                    if (File.Exists("patients.xml"))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Patient>));
                        using (FileStream fileStream = new FileStream("patients.xml", FileMode.Open))
                        {
                            patients = (List<Patient>)serializer.Deserialize(fileStream);
                        }
                    }

                    // Определение идентификатора для нового пациента
                    int nextId = 1;
                    if (patients.Count > 0)
                    {
                        nextId = patients[patients.Count - 1].Id + 1;
                    }

                    //Console.Write("Идентификатор: ");
                   // int id = int.Parse(Console.ReadLine());
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
                    Patient patient = new Patient(nextId, firstName, lastName, dateOfBirth, address, phoneNumber);
                    Console.WriteLine("Введите данные обследования:");
                    //Console.Write("Идентификатор: ");
                    //int examinationId = int.Parse(Console.ReadLine());
                    Console.Write("Дата (гггг-мм-дд): ");
                    DateTime examinationDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Результаты: ");
                    string examinationResults = Console.ReadLine();
                    Examination examination = new Examination(nextId, examinationDate, examinationResults);
                    patients.Add(patient);
                    patient.Examinations.Add(examination);
                    loadedPatients.AddRange(patients);

                    // Сохранение обновленного списка пациентов в файл
                    SaveToXml("patients.xml", loadedPatients);
                    Console.Clear();
                    Console.WriteLine("Список пациентов:");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Id     Имя          Фамилия ");
                    Console.ForegroundColor = ConsoleColor.White;
                    PrintPatientList(loadedPatients);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"═════════════════════════════════════════════════════════════════════════════════════════════════════");
                    Console.ForegroundColor = ConsoleColor.White;
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
}