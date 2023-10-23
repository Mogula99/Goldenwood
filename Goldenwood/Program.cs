using Goldenwood;
using Microsoft.EntityFrameworkCore;

using System;

namespace Goldenwood
{
    //TODO: Použít using na vytvoření dbContextu
    //TODO: Přidat funkcionalitu vložení nějakých dat do DB při spuštění aplikace
    public class Program
    {
        public static int playerId = 1;
        public static int playersArmyId = 1; 
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ApplicationDbContext dbContext = new ApplicationDbContext();
        }
    }
}



