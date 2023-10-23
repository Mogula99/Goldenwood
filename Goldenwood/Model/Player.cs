using Goldenwood.Model.Building;
using Goldenwood.Model.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Model
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GoldAmount { get; set; }
        public int WoodAmount { get; set; }
        public int TickInterval { get; set; }
        public Army Army { get; set; }
    }
}
