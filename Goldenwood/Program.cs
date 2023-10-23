using Goldenwood;
using Microsoft.EntityFrameworkCore;

using System;

namespace Goldenwood
{
    //TODO: Použít using na vytvoření dbContextu
    //TODO: Přidat funkcionalitu vložení nějakých dat do DB při spuštění aplikace
    //TODO: Zkontrolovat konzistenci camel case a 
    //TODO: Použít Unique atributy všude kde to dává smysl
    public class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ApplicationDbContext dbContext = new ApplicationDbContext();
        }
    }
}



