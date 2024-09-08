using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IMedicineTypeRepository : IBaseRepository<Model.MedicineTypeDto, MedicineType>, IInjectable
    {

    }
}
