using AutoMapper;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;

namespace EyeClinic.DataAccess.Repositories
{
    public class AccountPaymentCategoryRepository : BaseRepository<AccountPaymentCategoryDto, AccountPaymentCategory>, IAccountPaymentCategoryRepository
    {
        public AccountPaymentCategoryRepository(IMapper mapper, EyeClinicContext context) 
            : base(mapper, context) {
        }
    }
}
