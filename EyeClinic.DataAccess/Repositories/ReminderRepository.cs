using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;

namespace EyeClinic.DataAccess.Repositories
{
    public class ReminderRepository : BaseRepository<Model.ReminderDto, Reminder>, IReminderRepository
    {
        public ReminderRepository(IMapper mapper, EyeClinicContext context) : base(mapper, context) {
        }
    }
}
