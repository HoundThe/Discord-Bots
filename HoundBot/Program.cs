using System;
using System.Threading.Tasks;

namespace HoundBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                await new HoundBot().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(ex.HResult);
            }
        }
    }
}