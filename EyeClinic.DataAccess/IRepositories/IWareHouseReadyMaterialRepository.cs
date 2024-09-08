using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IWareHouseReadyMaterialRepository : IBaseRepository<Model.WareHouseReadyMaterialDto, WareHouseReadyMaterial>, IInjectable
    {
        public  Task<Model.WareHouseReadyMaterialDto> GetByTestId(TestDto testDto);
    }
}
