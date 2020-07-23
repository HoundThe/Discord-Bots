using System;
using System.Threading.Tasks;

namespace ApexBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                await new Bot().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(ex.HResult);
            }
        }
    }
}
