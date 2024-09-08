using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class VisitTypeRepository : BaseRepository<Model.VisitTypeDto, VisitType>, IVisitTypeRepository
    {
        public VisitTypeRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }
    }
}
