using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Model.Building
{
    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsBuilt { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int GoldCost { get; set; }
        public int WoodCost { get; set; }
    }
}
