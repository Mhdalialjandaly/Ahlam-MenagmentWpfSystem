using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IReminderRepository : IBaseRepository<Model.ReminderDto, Reminder>, IInjectable
    {

    }
}
