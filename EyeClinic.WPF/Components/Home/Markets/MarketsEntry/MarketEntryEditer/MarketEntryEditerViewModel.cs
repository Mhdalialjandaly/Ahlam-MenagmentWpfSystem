using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Markets.MarketsEntry.MarketEntryEditer
{
    public partial class MarketEntryEditerViewModel 
    {
        public MarketEntryEditerViewModel() {
            if (IsDesignMode)
            {

            }
        }

    }
    public partial class MarketEntryEditerViewModel:BaseViewModel<MarketEntryEditerView>
    {
        public readonly INotificationService _notificationService;
        public readonly IMarketsEntryRepository _marketsEntryRepository;
        public MarketEntryEditerViewModel(INotificationService notificationService,
            IMarketsEntryRepository marketsEntryRepository)
        {
            _notificationService = notificationService;
            _marketsEntryRepository = marketsEntryRepository;

            CreatedDate=DateTime.Now;
        }
        public int Id { get; set; }
        public double Quinttity { get; set; }
        public string UnitName { get; set; }
        public int CodeNumber { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public async Task<MarketsEntryDto> Save(MarketsProductsDto marketsProductsDto)
        {
            if (ValidForSave())
            {
                var item = BuildFromProperties(marketsProductsDto);
                if (Operation == operation.Add)
                {
                    var addedItem = await _marketsEntryRepository.Add(item);                    
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _marketsEntryRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave()
        {
            return CodeNumber != 0 || Quinttity != 0 || !string.IsNullOrWhiteSpace(UnitName);
        }

        private MarketsEntryDto BuildFromProperties(MarketsProductsDto marketsProductsDto)
        {
            return new()
            {
                Id = Id,
                Quinttity = Quinttity,
                CodeNumber = CodeNumber,
                UnitName = UnitName,
                Description = Description,
                CreatedDate = DateTime.Now,
                MarketsProductsId = marketsProductsDto.Id
            };
        }


        public void BuildFromModel(MarketsEntryDto marketsEntryDto)
        {
            Id = marketsEntryDto.Id;
            Quinttity = marketsEntryDto.Quinttity;
            CodeNumber = marketsEntryDto.CodeNumber;
            UnitName = marketsEntryDto.UnitName;
            Description = marketsEntryDto.Description;            
        }
    }
}
