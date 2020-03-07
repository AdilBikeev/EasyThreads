using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsProject
{
    class Program
    {
        /// <summary>
        /// Кол-во запускаемых потоков
        /// </summary>
        const int countThread = 5;

        /// <summary>
        /// Словарь потоков и соответствующих им времени жизни
        /// </summary>
        static Dictionary<Thread, int> threads;

        /// <summary>
        /// Объект для синхронизации потоков
        /// 1-ый аргумент отвечает за кол-во потоков, который будет хранить семафор
        /// 2-ой за максимальное допустимое число потоков, читающий один и тот же метод одновременно
        /// </summary>
        static Semaphore semaphore= new Semaphore(countThread, countThread);

        static void Main(string[] args)
        {
            Console.WriteLine("Старт основного потока");

            InitThreads();
            RunThreads();

            Console.WriteLine("Нажмите любую клавишу для завершение работы программы");
            Console.ReadKey();
            Console.WriteLine("Завершение основного потока");
        }

        /// <summary>
        /// Запускает все потоки
        /// </summary>
        private static void RunThreads()
        {
            foreach (var thread in threads.Keys)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Инициализирует все потоки
        /// </summary>
        private static void InitThreads()
        {
            if(threads == null)
            {
                threads = new Dictionary<Thread, int>();
            }

            try
            {
                for (int i = 0; i < Program.countThread; i++)
                {
                    Console.Write("Введите время жизни для потока в секундах: ");
                    int timeToLive = int.Parse(Console.ReadLine()) * 1000;

                    threads.Add(CreateThread(i.ToString()), timeToLive);
                }
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
        private static Thread CreateThread(string name)
        {
            Thread thread = null;

            try
            {
                thread = new Thread(new ThreadStart(TargetThreadMethod));
                thread.Name = name;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exception CreateThread[{name}]: {exc.Message}");
            }
            
            return thread;
        }
    
        /// <summary>
        /// Метод для потоков
        /// </summary>
        private static void TargetThreadMethod()
        {
            semaphore.WaitOne();//Блокируем поток если оно превышает макс. число допустимых одновременно читающих потоков

            Console.WriteLine($"Старт потока #{Thread.CurrentThread.Name}");
            Thread.Sleep(threads[Thread.CurrentThread]);
            Console.WriteLine($"Завершение потока #{Thread.CurrentThread.Name}");

            semaphore.Release();//освобождаем текущий поток
        }
    }
}
