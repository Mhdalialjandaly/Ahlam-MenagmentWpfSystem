using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Repositories
{
    public class CustomerNoteStoryRepository : BaseRepository<Model.CustomerNoteStoryDto, CustomerNoteStory>, IPublicCustomerNoteStoryRepository
    {
        public CustomerNoteStoryRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context)
        {
        }
    }
}
