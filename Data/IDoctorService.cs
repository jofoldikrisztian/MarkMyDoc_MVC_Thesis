using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IDoctorService
    {

        List<string> GetSearchResults(string toSearch);

    }
}
