﻿using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class DiseasesRepository : BaseRepository<Model.DiseaseDto, Disease>, IDiseasesRepository
    {
        public DiseasesRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
        }
    }
}