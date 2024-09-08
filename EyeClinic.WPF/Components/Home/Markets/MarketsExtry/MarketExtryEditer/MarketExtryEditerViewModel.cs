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

namespace EyeClinic.WPF.Components.Home.Markets.MarketsExtry.MarketExtryEditer
{
    public partial class MarketExtryEditerViewModel
    {
        public MarketExtryEditerViewModel()
        {
            if (IsDesignMode)
            {

            }
        }
    }
    public partial class MarketExtryEditerViewModel : BaseViewModel<MarketExtryEditerView>
    {
        public readonly INotificationService _notificationService;
        public readonly IMarketsExtryRepository _marketsExtryRepository;
        public MarketExtryEditerViewModel(INotificationService notificationService,
            IMarketsExtryRepository marketsExtryRepository)
        {
            _notificationService = notificationService;
            _marketsExtryRepository = marketsExtryRepository;

            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public double Quinttity { get; set; }
        public string UnitName { get; set; }
        public string Recipter { get; set; }
        public int CodeNumber { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public async Task<MarketsExtryDto> Save(MarketsProductsDto marketsProductsDto)
        {
            if (ValidForSave())
            {
                var item = BuildFromProperties(marketsProductsDto);
                if (Operation == operation.Add)
                {
                    var addedItem = await _marketsExtryRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _marketsExtryRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave()
        {
            return CodeNumber != 0 || Quinttity != 0 || !string.IsNullOrWhiteSpace(UnitName);
        }

        private MarketsExtryDto BuildFromProperties(MarketsProductsDto marketsProductsDto)
        {
            return new()
            {
                Id = Id,
                Quinttity = Quinttity,
                CodeNumber = CodeNumber,
                UnitName = UnitName,
                Recipter= Recipter,
                Description = Description,
                CreatedDate = DateTime.Now,
                MarketsProductsId = marketsProductsDto.Id
            };
        }


        public void BuildFromModel(MarketsExtryDto marketsExtryDto)
        {
            Id = marketsExtryDto.Id;
            Quinttity = marketsExtryDto.Quinttity;
            CodeNumber = marketsExtryDto.CodeNumber;
            UnitName = marketsExtryDto.UnitName;
            Recipter = marketsExtryDto.Recipter;
            Description = marketsExtryDto.Description;
        }
    }
}
