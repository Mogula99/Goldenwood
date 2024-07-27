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
        public virtual DbSet<EconomicBuilding> EconomicBuilding { get; set; }
        public virtual DbSet<MilitaryBuilding> MilitaryBuilding { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitGroup> UnitGroup { get; set; }
        public virtual DbSet<Army> Army { get; set; }
        public virtual DbSet<Enemy> Enemy { get; set; }
        public virtual DbSet<Player> Player {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source = gamedata.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().Navigation(e => e.Army).AutoInclude();
            modelBuilder.Entity<Enemy>().Navigation(e => e.Army).AutoInclude();
            modelBuilder.Entity<Army>().Navigation(e => e.UnitGroups).AutoInclude();
            modelBuilder.Entity<UnitGroup>().Navigation(e => e.Unit).AutoInclude();
        }
    }
}
