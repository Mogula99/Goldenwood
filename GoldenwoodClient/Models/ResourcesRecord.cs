using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Models
{
    public partial class ResourcesRecord : ObservableObject
    {
        [ObservableProperty] private int goldAmount;
        [ObservableProperty] private int woodAmount;
    }
}
