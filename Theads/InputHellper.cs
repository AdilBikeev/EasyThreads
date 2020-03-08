using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadsProject
{
    internal static class InputHellper
    {
        /// <summary>
        /// Возвращает число, вводимое пользователем, блокируя возможность использовать другие символы
        /// </summary>
        /// <returns></returns>
        internal static string GetNum()
        {
            string num = string.Empty;
            int currentPos = 0;
            char symbol;

            do
            {
                symbol = Console.ReadKey(true).KeyChar;
                if (char.IsDigit(symbol))
                {
                    num += symbol;
                    currentPos++;
                    Console.Write(symbol);
                }else if(symbol == '\b' && currentPos > 0)
                {
                    Backspace();
                    currentPos--;
                }
            } while (symbol != '\r' || string.IsNullOrEmpty(num));

            return num;
        }

        private static void Backspace()
        {
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}
