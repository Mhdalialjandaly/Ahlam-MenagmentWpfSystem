using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class LabTestRepository : BaseRepository<Model.LabTestDto, LabTest>, ILabTestRepository
    {
        public LabTestRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
