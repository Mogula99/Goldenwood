using Goldenwood;
using Goldenwood.Service;
using Microsoft.EntityFrameworkCore;

using System;

namespace Goldenwood
{
    public class Program
    {        
        static void Main()
        {
            Console.WriteLine("Hello, World!");
            
            using ApplicationDbContext dbContext = new ApplicationDbContext();
            ResourcesService resourcesService = new ResourcesService(dbContext);
            BuildingService buildingService = new BuildingService(dbContext, resourcesService);
            MilitaryService militaryService = new MilitaryService(dbContext, resourcesService, buildingService);
        }
    }
}



