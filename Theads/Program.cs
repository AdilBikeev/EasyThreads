using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Start();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Запускает решение дял лаб. работы
        /// </summary>
        private static void Start()
        {
            ThreadModule.InitThreads();
            ThreadModule.RunThreads();
        }

        /// <summary>
        /// Метод для потоков
        /// </summary>
        internal static void TargetThreadMethod()
        {
            try
            {
                ThreadModel.semaphore.WaitOne();//Блокируем поток если оно превышает макс. число допустимых одновременно читающих потоков

                Console.WriteLine($"Старт потока #{Thread.CurrentThread.Name}");
                Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);
                Console.WriteLine($"Завершение потока #{Thread.CurrentThread.Name}");

                ThreadModel.semaphore.Release();//освобождаем текущий поток
                ThreadModel.threads.Remove(Thread.CurrentThread);

                if(ThreadModel.threads.Count == 0)
                {
                    Console.WriteLine("\n\n\n");
                    Console.Write("Продолжить запуск потоков ?[Y/N]: ");
                    var ans = Console.ReadKey().KeyChar;
                    if(ans == 'Y')
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        Start();
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"TargetThreadMethod exception: {exc.Message}");
            }
        }
    }
}
