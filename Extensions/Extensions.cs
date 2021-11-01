using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MarkMyDoctor.Extensions
{
    public static class Extensions
    {

        public async static Task<List<char>> GetLetters<T>(this IQueryable<T> source)
        {
            var characters = new List<char>();

            if (typeof(T) == typeof(Doctor))
            {

                var doctors = await source.Cast<Doctor>().ToListAsync();

                if (doctors.Count > 0)
                {
                    foreach (var item in doctors)
                    {
                        if (!characters.Contains(item.Name[0]))
                        {
                            characters.Add(item.Name[0]);
                        }
                    }
                }
            }
            return characters;
        }
    }
}
