using MarkMyDoctor.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Infrastructure
{
    public class PaginatedList<Doctor> : List<Doctor>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool ByName { get; set; }
        public bool ByCity { get; set; }
        public bool BySpeciality { get; set; }
        public IEnumerable<char> Characters { get; set; }

        private PaginatedList(IEnumerable<Doctor> items, int count, int pageIndex, int pageSize, bool byName, bool byCity, bool bySpeciality, IEnumerable<char> characters)
        {
            PageIndex = pageIndex;
            ByName = byName;
            ByCity = byCity;
            this.BySpeciality = bySpeciality;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
            Characters = characters;
        }

        public bool PreviousPage => (PageIndex > 1);
        public bool NextPage => (PageIndex < TotalPages);

        public static async Task<PaginatedList<Doctor>> CreateAsync(IQueryable<Doctor> source, int pageIndex, int pageSize, bool byName = true, bool byCity = true, bool bySpeciality = true)
        {
            var count = await source.CountAsync();

           

            var letters = await source.GetLetters();

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<Doctor>(items, count, pageIndex, pageSize, byName, byCity, bySpeciality, letters);
        }

   

    }
}