using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadsProject
{
    internal static class ThreadModule
    {
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
                for (int i = 0; i < ThreadModel.countThread; i++)
                {
                    Console.Write($"Введите время жизни для потока #{i} в секундах: ");
                    int timeToLive = int.Parse(InputHellper.GetNum()) * 1000;
                    Console.WriteLine();

                    ThreadModel.threads.Add(CreateThread(i.ToString()), timeToLive);
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
    }
}
