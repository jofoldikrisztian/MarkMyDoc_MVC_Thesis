using MarkMyDoctor.Data;
using MarkMyDoctor.Middlewares;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MarkMyDoctor.Extensions
{
    public static class Extensions
    {
        public async static Task<List<char>> GetLettersAsync<T>(this IQueryable<T> source)
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

        public async static Task<int> GetDoctorIndex<T>(this IQueryable<T> source, char character)
        {
            var index = 1;

            if (typeof(T) == typeof(Doctor))
            {

                var doctors = await source.Cast<Doctor>().ToListAsync();

                var result = doctors.Select((x, i) => new { Doctor = x, Index = i })
                                    .Where(itemWithIndex => itemWithIndex.Doctor.Name.StartsWith(character))
                                    .FirstOrDefault();

                index = result.Index + 1;

            }
            return index;
        }

        public static IApplicationBuilder UseUserChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserCheckerMiddleware>();
        }

    }
}
