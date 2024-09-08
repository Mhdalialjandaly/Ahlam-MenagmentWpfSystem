using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.IRepositories
{
    internal interface IPublicCustomerNoteStoryRepository : IBaseRepository<Model.CustomerNoteStoryDto, CustomerNoteStory>, IInjectable
    {
    }
}
