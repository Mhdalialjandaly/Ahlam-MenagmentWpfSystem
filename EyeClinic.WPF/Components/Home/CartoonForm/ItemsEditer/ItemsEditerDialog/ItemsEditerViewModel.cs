using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor;
using LFSO102Lib;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer.ItemsEditerDialog
{
    public partial class ItemsEditerViewModel
    {
        public ItemsEditerViewModel() { }
    }

    public partial class ItemsEditerViewModel : BaseViewModel<ItemsEditerView>
    {
        private readonly ICartoonLabelsRepository _cartoonItemRepository;
       
        public ItemsEditerViewModel(ICartoonLabelsRepository cartoonItemRepository)
        {
            _cartoonItemRepository = cartoonItemRepository;
         
           
        }
        public int Id { get; set; }
        public int Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ThePaperCode { set; get; }
        public double DEntry { set; get; }
        public double DExtry { set; get; }
        public int MinimumValue { set; get; }
        public int  FirstDayValue { set; get; }
        public string Note { set; get; }
        public double CurrentValue { get; set; }        
        public DateTime DateOf { get; set; } = DateTime.Now;



        public async Task<CartoonLabelsDto> Save(DiseaseDto disease)
        {
            CurrentValue = await _cartoonItemRepository.GetSumCartoonLabels(disease.Id);
            if (ValidForSave())
            {
                var item = BuildFromProperties(disease);              
                if (Operation == operation.Add)
                {
                    var addedItem = await _cartoonItemRepository.Add(item);   
                    return addedItem;

                    
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _cartoonItemRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave()
        {
            return DEntry != 0 || DExtry != 0 || ThePaperCode != 0;
        }

        private CartoonLabelsDto BuildFromProperties(DiseaseDto disease)
        {
            return new()
            {
                Id = Id,
                CartoonNameId = disease.Id,
                DEntry = DEntry,
                DExtry = DExtry,
                CreatedDate = DateOf.Date,
                ThePaperCode = ThePaperCode,
                Note = Note,
                CurrentValue = CurrentValue
            };
        }
       

        public void BuildFromModel(CartoonLabelsDto disease)
        {
            Id = disease.Id;
            Code = disease.CartoonNameId;
            DEntry = disease.DEntry;
            DExtry = disease.DExtry;
            CreatedDate = disease.CreatedDate.Date;
            ThePaperCode = disease.ThePaperCode;           
            Note = disease.Note;
            DateOf = disease.CreatedDate;
            CurrentValue = disease.CurrentValue;
        }       
      
    }
}
