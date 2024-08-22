
using KeyLogger.Services;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            KeyLoggerService keyLoggerService = new KeyLoggerService();

            keyLoggerService.Start((key) =>
            {
                Console.WriteLine(key);
            });

            // Wait ctrl + c to stop the keylogger
            Console.WriteLine("Press ctrl + c to stop the keylogger");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.C && ConsoleModifiers.Control != 0)
                {
                    keyLoggerService.Stop();
                    break;
                }
            }
        }
    }
}