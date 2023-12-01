using CommunityToolkit.Mvvm.ComponentModel;
using Goldenwood.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Models
{
    public partial class UnitGroup : ObservableObject
    {
        [ObservableProperty] private int id;
        [ObservableProperty] private Unit unit;
        [ObservableProperty] private int unitCount;
    }
}
