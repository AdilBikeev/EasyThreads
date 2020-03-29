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
        /// Объект для синхронизации потоков/
        /// </summary>
        internal static Mutex mutex = new Mutex();

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
    }
}
