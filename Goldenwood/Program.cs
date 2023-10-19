using Goldenwood;
using Microsoft.EntityFrameworkCore;

using System;

namespace Goldenwood // Note: actual namespace depends on the project name.
{
    //TODO: Použít using na vytvoření dbContextu
    //TODO: Vytvořit nějakou globální konstantu na playerID
    //TODO: Přidat funkcionalitu vložení nějakých dat do DB od spuštění aplikace
    public class Program
    {
        public static int playerId = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ApplicationDbContext dbContext = new ApplicationDbContext();
        }
    }
}



