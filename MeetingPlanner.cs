using System;
using System.Collections.Generic;
using System.IO;

/*
Разработать консольное приложение на C# .Net по описанию. 
Для приложения написать план тестирования.
Исходные данные:
Необходимо разработать приложение для управления личными встречами.
Встреча – событие, которое планируется заранее, для которой всегда известно время начала и примерное время окончания. 
Данные о встречах могут быть добавлены, изменены и удалены. Встречи всегда планируются только на будущее время. При этом встречи не должны пересекаться друг с другом. 
Также для встречи может быть настроено время, за которое нужно уведомить пользователя о предстоящей встрече. Время напоминания также может быть изменено после создания встречи. При наступлении времени напоминания приложение информирует пользователя о предстоящей встрече и времени ее начала.
Пользователь может посмотреть расписание своих встреч на любой день, в том числе и прошедший. 
Помимо просмотра он может с помощью приложения экспортировать расписание встреч за выбранный день в текстовый файл.
*/

namespace MeetingPlanner
{
    class Program
    {
        static List<Meeting> meetings = new List<Meeting>();

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Просмотреть расписание встреч на день");
                Console.WriteLine("2. Добавить встречу");
                Console.WriteLine("3. Изменить встречу");
                Console.WriteLine("4. Удалить встречу");
                Console.WriteLine("5. Экспортировать расписание в файл");
                Console.WriteLine("6. Выйти");

                string? input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "1":
                        ViewMeetings();
                        break;
                    case "2":
                        AddMeeting();
                        break;
                    case "3":
                        EditMeeting();
                        break;
                    case "4":
                        DeleteMeeting();
                        break;
                    case "5":
                        ExportSchedule();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewMeetings()
        {
            Console.WriteLine("Введите дату для просмотра расписания (в формате ГГГГ-ММ-ДД):");
            string? input = Console.ReadLine();

            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                List<Meeting> meetingsOnDate = meetings.FindAll(m => m.StartTime.Date == date.Date);
                if (meetingsOnDate.Count == 0)
                {
                    Console.WriteLine("На этот день нет запланированных встреч.");
                }
                else
                {
                    Console.WriteLine($"Расписание встреч на {date.ToShortDateString()}:");
                    foreach (Meeting meeting in meetingsOnDate)
                    {
                        Console.WriteLine($"- {meeting.StartTime.ToShortTimeString()} - {meeting.EndTime.ToShortTimeString()} ({meeting.ReminderMinutes} мин. напоминание)");
                    }
                }
            }
            else
            {
                Console.WriteLine("Неверный формат даты.");
            }
        }

        static void AddMeeting()
        {
            Console.WriteLine("Введите время начала встречи (в формате ЧЧ:ММ):");
            string? startTimeInput = Console.ReadLine();

            Console.WriteLine("Введите примерное время окончания встречи (в формате ЧЧ:ММ):");
            string? endTimeInput = Console.ReadLine();

            Console.WriteLine("Введите время напоминания (в минутах):");
            string? reminderInput = Console.ReadLine();

            DateTime startTime, endTime;
            int reminderMinutes;
            if (DateTime.TryParse(startTimeInput, out startTime) && DateTime.TryParse(endTimeInput, out endTime) && int.TryParse(reminderInput, out reminderMinutes))
            {
                if (endTime > startTime)
                {
                    Meeting newMeeting = new Meeting(startTime, endTime, reminderMinutes);
                    if (IsMeetingOverlap(newMeeting))
                    {
                        Console.WriteLine("Встреча пересекается с другими встречами. Пожалуйста, выберите другое время.");
                    }
                    else
                    {
                        meetings.Add(newMeeting);
                        Console.WriteLine("Встреча успешно добавлена.");
                    }
                }
                else
                {
                    Console.WriteLine("Время окончания должно быть позже времени начала.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ввода.");
            }
        }

        static void EditMeeting()
        {
            Console.WriteLine("Введите номер встречи для изменения:");
            string? indexInput = Console.ReadLine();

            int index;
            if (int.TryParse(indexInput, out index) && index >= 0 && index < meetings.Count)
            {
                Meeting meeting = meetings[index];

                Console.WriteLine("Введите новое время начала встречи (в формате ЧЧ:ММ):");
                string? startTimeInput = Console.ReadLine();

                Console.WriteLine("Введите новое примерное время окончания встречи (в формате ЧЧ:ММ):");
                string? endTimeInput = Console.ReadLine();

                Console.WriteLine("Введите новое время напоминания (в минутах):");
                string? reminderInput = Console.ReadLine();

                DateTime startTime, endTime;
                int reminderMinutes;
                if (DateTime.TryParse(startTimeInput, out startTime) && DateTime.TryParse(endTimeInput, out endTime) && int.TryParse(reminderInput, out reminderMinutes))
                {
                    if (endTime > startTime)
                    {
                        Meeting updatedMeeting = new Meeting(startTime, endTime, reminderMinutes);
                        if (IsMeetingOverlap(updatedMeeting, meeting))
                        {
                            Console.WriteLine("Встреча пересекается с другими встречами. Пожалуйста, выберите другое время.");
                        }
                        else
                        {
                            meeting.StartTime = startTime;
                            meeting.EndTime = endTime;
                            meeting.ReminderMinutes = reminderMinutes;
                            Console.WriteLine("Встреча успешно изменена.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Время окончания должно быть позже времени начала.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат ввода.");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер встречи.");
            }
        }

        static void DeleteMeeting()
        {
            Console.WriteLine("Введите номер встречи для удаления:");
            string? indexInput = Console.ReadLine();

            int index;
            if (int.TryParse(indexInput, out index) && index >= 0 && index < meetings.Count)
            {
                meetings.RemoveAt(index);
                Console.WriteLine("Встреча успешно удалена.");
            }
            else
            {
                Console.WriteLine("Неверный номер встречи.");
            }
        }

        static void ExportSchedule()
        {
            Console.WriteLine("Введите дату для экспорта расписания (в формате ГГГГ-ММ-ДД):");
            string? input = Console.ReadLine();

            DateTime date;
            if (DateTime.TryParse(input, out date))
            {
                string fileName = $"Расписание_{date.ToShortDateString()}.txt";
                List<Meeting> meetingsOnDate = meetings.FindAll(m => m.StartTime.Date == date.Date);

                if (meetingsOnDate.Count == 0)
                {
                    Console.WriteLine("На этот день нет запланированных встреч.");
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        writer.WriteLine($"Расписание встреч на {date.ToShortDateString()}:");
                        foreach (Meeting meeting in meetingsOnDate)
                        {
                            writer.WriteLine($"- {meeting.StartTime.ToShortTimeString()} - {meeting.EndTime.ToShortTimeString()} ({meeting.ReminderMinutes} мин. напоминание)");
                        }
                    }

                    Console.WriteLine($"Расписание успешно экспортировано в файл {fileName}.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат даты.");
            }
        }

        static bool IsMeetingOverlap(Meeting newMeeting, Meeting? existingMeeting = null)
        {
            foreach (Meeting meeting in meetings)
            {
                if (meeting != existingMeeting && (newMeeting.StartTime >= meeting.StartTime && newMeeting.StartTime < meeting.EndTime ||
                                                  newMeeting.EndTime > meeting.StartTime && newMeeting.EndTime <= meeting.EndTime))
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Meeting
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ReminderMinutes { get; set; }

        public Meeting(DateTime startTime, DateTime endTime, int reminderMinutes)
        {
            StartTime = startTime;
            EndTime = endTime;
            ReminderMinutes = reminderMinutes;
        }
    }
}
