using Goldenwood.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Service
{
    public class PlayerService
    {
        private ApplicationDbContext dbContext;

        public PlayerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
