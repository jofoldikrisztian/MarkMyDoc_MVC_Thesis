using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IUnitOfWork
    {
        IDoctorRepository DoctorRepository { get; }
        IReviewRepository ReviewRepository { get; }
        void Save();
    }
}
