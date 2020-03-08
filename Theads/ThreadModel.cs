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
    }
}
