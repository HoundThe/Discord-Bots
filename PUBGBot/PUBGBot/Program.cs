using System;
using System.Threading.Tasks;

namespace PUBGBot
{
    internal static class Program
    {
        private static async Task Main()
        {
            try
            {
                await new PubgBot().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(ex.HResult);
            }
        }
    }
}