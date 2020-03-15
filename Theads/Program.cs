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

            ThreadModule.WaitHandler();

            while (ThreadModel.threads.Count != 0) Thread.Sleep(1);

            ThreadModel.semaphore.Dispose();
            ThreadModel.semaphore.Close();
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
            var isFirstStart = true;
            while (!Program.isTheEnd)
            {
                if (isFirstStart ||  Program.goRestart)
                {
                    isFirstStart = false;
                    try
                    {
                        ThreadModel.semaphore.WaitOne();//Блокируем поток если оно превышает макс. число допустимых одновременно читающих потоков
                        Console.WriteLine($"Старт потока #{Thread.CurrentThread.Name}");
                        Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);
                        Console.WriteLine($"Поток #{Thread.CurrentThread.Name} закончил работу");
                        
                        countFinishedThreads++;

                        Program.isStop = true;
                        while (Program.isStop)
                        {
                            Thread.Sleep(1);
                        }
                        ThreadModel.semaphore.Release();//освобождаем текущий поток
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"TargetThreadMethod exception: {exc.Message}");
                    }
                }
                
            }

            Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);//перед завершением потока - останавливаем поток на некоторое время
            Console.WriteLine($"Поток #{Thread.CurrentThread.Name} ЗАВЕРШИЛ работу");
            ThreadModel.threads.Remove(Thread.CurrentThread);
        }
    }
}
