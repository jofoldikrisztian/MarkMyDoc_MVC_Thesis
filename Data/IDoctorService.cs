﻿using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IDoctorService
    {


        Task<PaginatedList<Doctor>> GetDoctorsByCityAsync(string toSearch, int pageNumber);

        Task<PaginatedList<Doctor>> GetDoctorsByNameAsync(string toSearch, int pageNumber);
        Task<PaginatedList<Doctor>> GetAllDoctorAsync(int pageNumber);
      
        Task<PaginatedList<Doctor>> GetDoctorsBySpeciality(string toSearch, int pageNumber);
        Task<Doctor> GetDoctorByIdAsync(int? id);
        bool DoctorExists(int id);
        Task SaveChangesAsync();
        void AddDoctor(Doctor doctor);
        void UpdateDoctor(Doctor doctor);
        void Remove(Doctor doctor);
        Task<ICollection<SelectListItem>> GetSpecialitiesToSelectListAsync();
        Task<PaginatedList<Doctor>> GetDoctorSearchResult(string toSearch, int pageNumber);
        Task<ICollection<Speciality>> GetSpecialitiesAsync();
        IAsyncEnumerable<Speciality> GetSelectedSpecialitiesAsync(List<string> selectedSpecialityIds);
        Task<ICollection<DoctorSpeciality>> GetDoctorSpecialities(int id);
        Task CalculateDoctorOverall(int id, int actualReviewScore);
        Task CreateReview(Review review);
        Task<User> GetUserById(int id);
        Task CreateDoctor(Doctor doctor);
        Task AddDoctorSpecialities(List<DoctorSpeciality> docSpecList);
    }
}
