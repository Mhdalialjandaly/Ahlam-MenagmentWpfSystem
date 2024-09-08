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
    public interface IPublicCustomerRepository : IBaseRepository<Model.PublicCustomerDto, PublicCustomer>, IInjectable
    {
        //Task<Model.PatientDto> GetByPatientVisitId(int patientVisitId);
        public  Task<List<Model.PublicCustomerDto>> Search(string code, string firstName, string lastName);
        //Task UpdateNote(int patientId, string specialNote);


    }
}
