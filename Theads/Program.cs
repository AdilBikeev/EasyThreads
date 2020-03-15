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
        static bool isTheEnd = false;

        /// <summary>
        /// Сигнализирует всем потокам о перезапуске
        /// </summary>
        static bool goRestart = false;

        /// <summary>
        /// Указывает, что все потоки закончили свою работу
        /// </summary>
        static bool allThreadsIsFinished = false;

        /// <summary>
        /// Кол-во потоков, заканчивших свою работу
        /// </summary>
        static int countFinishedThreads = 0;


        static bool isStop = true;
        
        public static AutoResetEvent evt = new AutoResetEvent(true);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Start();

            do {
                Program.isTheEnd = false;

                //Thread.Sleep(ThreadModel.GetMaxTimeToLive());// Ждем завершения всех потоков
                

                // Если все потоки закончили свою работу
                if (Program.countFinishedThreads == ThreadModel.threads.Count)
                {
                    Console.WriteLine("\n\n\n");
                    Console.Write("Продолжить запуск потоков ?[Y/N]: ");
                    var ans = InputHellper.GetAnswer();
                    Program.countFinishedThreads = 0;
                    Program.allThreadsIsFinished = true;
                    Console.WriteLine();
                    if (ans == 'Y')
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        Restart();
                        Program.isTheEnd = false;
                        Program.goRestart = true;
                    }
                    else
                    {
                        Program.isTheEnd = true;
                        Program.goRestart = false;                    
                    }
                    Program.isStop = false;
                } else
                {
                    Program.allThreadsIsFinished = false;
                }
            } while(! isTheEnd);
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
        /// Перезапускает работу потоков изменяя время их жизни
        /// </summary>
        private static void Restart()
        {

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
