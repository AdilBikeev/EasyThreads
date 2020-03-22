using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace ThreadsProject
{
    internal static class ThreadModule
    {
        /// <summary>
        /// Максимальное время жизни потока
        /// </summary>
        private const int maxTimeToLive = 5;

        /// <summary>
        /// Сигнализирует главному потоку - нужно ли задать вопрос пользователю или нет
        /// </summary>
        internal static bool checkAns = false;

        /// <summary>
        /// Запускает все потоки
        /// </summary>
        internal static void RunThreads()
        {
            try
            {
                foreach (var thread in ThreadModel.threads.Keys)
                {
                    thread.Start();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"RunThreads exception: {exc.Message}");
            }
        }

        /// <summary>
        /// Инициализирует все потоки
        /// </summary>
        internal static void InitThreads()
        {
            if (ThreadModel.threads == null)
            {
                ThreadModel.threads = new Dictionary<Thread, int>();
            }

            try
            {
                Random random = new Random();

                for (int i = 0; i < ThreadModel.countThread; i++)
                {
                    Console.Write($"Время жизни для потока #{i} в секундах: ");
                    int timeToLive = ((int)(random.NextDouble()*1000 % 20 + 1) * (i + 1)) % maxTimeToLive; // ставим ограничение, чтобы поток не длился более maxTimeToLive секунд
                    Console.WriteLine(timeToLive);

                    ThreadModel.threads.Add(CreateThread(i.ToString()), timeToLive * 1000);
                }
                Console.WriteLine("\n\n\n");
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exception InitThreads[{Thread.CurrentThread.Name}]: {exc.Message}");
            }
        }

        /// <summary>
        /// Возвращает созданный поток с указанным именем
        /// </summary>
        /// <param name="name">Имя потока</param>
        /// <returns></returns>
        internal static Thread CreateThread(string name)
        {
            Thread thread = null;

            try
            {
                thread = new Thread(new ThreadStart(Program.TargetThreadMethod));
                thread.Name = name;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exception CreateThread[{name}]: {exc.Message}");
            }

            return thread;
        }

        /// <summary>
        /// Метод для ожидания завершения всех потоков
        /// </summary>
        internal static void WaitHandler()
        {
            do
            {
                Program.isTheEnd = false;

                // Если все потоки закончили свою работу
                if (ThreadModule.checkAns && Program.countFinishedThreads == ThreadModel.threads.Count)
                {
                    Console.WriteLine("\n\n\n");
                    Console.Write("Продолжить запуск потоков ?[Y/N]: ");
                    var ans = InputHellper.GetAnswer();
                    Console.WriteLine();
                    if (ans == 'Y')
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        ThreadModule.Restart();
                        Program.isTheEnd = false;
                        Program.goRestart = true;
                    }
                    else
                    {
                        Program.isTheEnd = true;
                        Program.goRestart = false;
                    }
                    Program.countFinishedThreads = 0;
                    Program.isStop = false;

                    ThreadModule.checkAns = false;
                }
            } while (!Program.isTheEnd);
        }

        /// <summary>
        /// Перезапускает работу потоков изменяя время их жизни
        /// </summary>
        internal static void Restart()
        {
            try
            {
                Random random = new Random();

                var lstDict = ThreadModel.threads.Keys.ToList();

                foreach (var thread in lstDict)
                {
                    Console.Write($"Время жизни для потока #{thread.Name} в секундах: ");
                    int timeToLive = ((int)(random.NextDouble() * 1000 % 20 + 1) * (int.Parse(thread.Name) + 1)) % maxTimeToLive; // ставим ограничение, чтобы поток не длился более maxTimeToLive секунд
                    Console.WriteLine(timeToLive);
                    ThreadModel.threads[thread] = timeToLive * 1000;
                }
                Console.WriteLine("\n\n\n");
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exception InitThreads[{Thread.CurrentThread.Name}]: {exc.Message}");
            }
        }
    }
}
