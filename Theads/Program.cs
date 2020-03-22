using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadsProject
{
    internal class Program
    {
        /// <summary>
        /// Флаг указывающий потокам о завершении работы
        /// </summary>
        internal static bool isTheEnd = false;

        /// <summary>
        /// Сигнализирует всем потокам о перезапуске
        /// </summary>
        internal static bool goRestart = false;

        /// <summary>
        /// Кол-во потоков, заканчивших свою работу
        /// </summary>
        internal static int countFinishedThreads = 0;

        /// <summary>
        /// Указывают потоком приостановить работу
        /// </summary>
        internal static bool isStop = true;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Start();

            while (ThreadModel.threads.Count != 0) Thread.Sleep(1);

            ThreadModel.mutex.Dispose();
            ThreadModel.mutex.Close();
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
            while (!Program.isTheEnd)
            {
                ThreadModel.mutex.WaitOne();//Блокируем поток если оно превышает макс. число допустимых одновременно читающих потоков

                if(!Program.isTheEnd)
                {
                    Console.WriteLine($"Старт потока #{Thread.CurrentThread.Name}");
                    Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);
                    Console.WriteLine($"Поток #{Thread.CurrentThread.Name} закончил работу");
                    countFinishedThreads++;
                    if (countFinishedThreads == 5)
                    {
                        countFinishedThreads = 0;
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
                        }
                        else
                        {
                            Program.isTheEnd = true;
                        }
                    }
                }
                ThreadModel.mutex.ReleaseMutex();
            }


            try
            {
                ThreadModel.mutex.WaitOne();//Блокируем поток если оно превышает макс. число допустимых одновременно читающих потоков
                Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);//перед завершением потока - останавливаем поток на некоторое время
                Console.WriteLine($"Поток #{Thread.CurrentThread.Name} ЗАВЕРШИЛ работу");
                ThreadModel.threads.Remove(Thread.CurrentThread);
                ThreadModel.mutex.ReleaseMutex();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Exceptrion [{Thread.CurrentThread.Name}]: {exc.Message}");
            }

        }
    }
}
