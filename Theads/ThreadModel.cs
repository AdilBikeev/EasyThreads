using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadsProject
{
    internal static class ThreadModel
    {
        /// <summary>
        /// Кол-во запускаемых потоков
        /// </summary>
        internal const int countThread = 5;

        /// <summary>
        /// Словарь потоков и соответствующих им времени жизни
        /// </summary>
        internal static Dictionary<Thread, int> threads;

        /// <summary>
        /// Объект для синхронизации потоков
        /// 1-ый аргумент отвечает за кол-во потоков, который будет хранить семафор
        /// 2-ой за максимальное допустимое число потоков, читающий один и тот же метод одновременно
        /// </summary>
        internal static Semaphore semaphore = new Semaphore(countThread, countThread);

        /// <summary>
        /// Возвращает максимальное время жизни среди всех созданных потоков
        /// </summary>
        /// <returns></returns>
        internal static int GetMaxTimeToLive()
        {
            int max = 0;
            foreach (var thread in threads)
            {
                if(thread.Value > max)
                {
                    max = thread.Value;
                }
            }
            return max;
        }
    
        /// <summary>
        /// Метод для ожидания завершения всех потоков
        /// </summary>
        internal static void WaitHandler()
        {
            do
            {
                Program.isTheEnd = false;

                //Thread.Sleep(ThreadModel.GetMaxTimeToLive());// Ждем завершения всех потоков


                // Если все потоки закончили свою работу
                if (Program.countFinishedThreads == ThreadModel.threads.Count)
                {
                    Console.WriteLine("\n\n\n");
                    Console.Write("Продолжить запуск потоков ?[Y/N]: ");
                    var ans = InputHellper.GetAnswer();
                    Program.countFinishedThreads = 0;
                    Console.WriteLine();
                    if (ans == 'Y')
                    {
                        Console.WriteLine("\nНажмите любую клавишу для продолжения");
                        Console.ReadKey();
                        Console.Clear();
                        Program.Restart();
                        Program.isTheEnd = false;
                        Program.goRestart = true;
                    }
                    else
                    {
                        Program.isTheEnd = true;
                        Program.goRestart = false;
                    }
                    Program.isStop = false;
                }
            } while (!Program.isTheEnd);
        }
    }
}
