﻿using System;
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
            var isFirstStart = true;
            while (!Program.isTheEnd)
            {
                if (isFirstStart ||  Program.goRestart)
                {
                    isFirstStart = false;
                    try
                    {
                        Console.WriteLine($"Старт потока #{Thread.CurrentThread.Name} (Время: {DateTime.Now.ToString("mm мин. ss с.  fffff мс.")})");
                        Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);
                        Console.WriteLine($"Поток #{Thread.CurrentThread.Name} закончил работу (Время: {DateTime.Now.ToString("mm мин. ss с.  fffff мс.")})");
                        
                        countFinishedThreads++;

                        Program.isStop = true;
                        while (Program.isStop)
                        {
                            Thread.Sleep(1);
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"TargetThreadMethod exception: {exc.Message}");
                    }
                }
                
            }

            Console.WriteLine($"Начало завершения потока #{Thread.CurrentThread.Name} (Время: {DateTime.Now.ToString("mm мин. ss с.  fffff мс.")})");
            Thread.Sleep(ThreadModel.threads[Thread.CurrentThread]);//перед завершением потока - останавливаем поток на некоторое время
            Console.WriteLine($"Поток #{Thread.CurrentThread.Name} ЗАВЕРШИЛ работу (Время: {DateTime.Now.ToString("mm мин. ss с.  fffff мс.")})");
            ThreadModel.threads.Remove(Thread.CurrentThread);
        }
    }
}
