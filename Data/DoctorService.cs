using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class DoctorService : IDoctorService
    {
        public DoctorService(DoctorDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public DoctorDbContext DbContext { get; }


    }
}
