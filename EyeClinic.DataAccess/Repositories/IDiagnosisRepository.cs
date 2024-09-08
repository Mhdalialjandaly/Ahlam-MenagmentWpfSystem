using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class DiagnosisRepository : BaseRepository<Model.DiagnosisDto, Diagnosis>, IDiagnosisRepository
    {
        public DiagnosisRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context) {
        }

        public override async Task<List<Model.DiagnosisDto>> GetAll() {
            var items = await base.GetAll();
            return items.OrderBy(e => e.EnName).ToList();
        }
    }
}
