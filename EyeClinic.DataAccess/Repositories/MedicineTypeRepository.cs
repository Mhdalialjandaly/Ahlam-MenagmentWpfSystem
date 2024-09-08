using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class MedicineTypeRepository : BaseRepository<Model.MedicineTypeDto, MedicineType>, IMedicineTypeRepository
    {
        public MedicineTypeRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
