using Goldenwood.Model;
using Goldenwood.Model.Building;
using Goldenwood.Model.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EconomicBuilding> EconomicBuildings { get; set; }
        public DbSet<MilitaryBuilding> MilitaryBuildings{ get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Player> Player {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source = C:\Users\mach2\FIT\DNP\Goldenwood\Goldenwood\gamedata.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().Navigation(e => e.Army).AutoInclude();
            modelBuilder.Entity<Player>().Navigation(e => e.EconomicBuildings).AutoInclude();
            modelBuilder.Entity<Player>().Navigation(e => e.MilitaryBuildings).AutoInclude();
            modelBuilder.Entity<Enemy>().Navigation(e => e.Army).AutoInclude();
            modelBuilder.Entity<Army>().Navigation(e => e.UnitGroups).AutoInclude();
            modelBuilder.Entity<UnitGroup>().Navigation(e => e.Unit).AutoInclude();
            modelBuilder.Entity<MilitaryBuilding>().Navigation(e => e.CreatableUnits).AutoInclude();
        }
    }
}
