using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Markets.MarketsProductEditer
{
    public partial class MarketsProductEditerViewModel 
    {
        public MarketsProductEditerViewModel() { }

       
    }
    public partial class MarketsProductEditerViewModel:BaseViewModel<MarketsProductEditerView>
    {
        public readonly INotificationService _notificationService;
        public readonly IUnityContainer _unityContainer;
        public readonly IMarketsProductsRepository _marketsProductsRepository;
        
        public  MarketsProductEditerViewModel(INotificationService notificationService,IMarketsProductsRepository marketsProductsRepository,IUnityContainer unityContainer)
        {
            _notificationService = notificationService;
            _unityContainer = unityContainer;
            _marketsProductsRepository = marketsProductsRepository;

            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string MarketsProductName { get; set; }
        public double TheMinimumValue { get; set; }
        public double FirstTermValue { get; set; }
        public double Entry { get; set; }
        public double Extry { get; set; }
        public double RealValue { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get;  set; }

        public async Task<MarketsProductsDto> Save()
        {           
            if (ValidForSave())
            {
                var item = BuildFromProperties();
                if (Operation == operation.Add)
                {
                    var addedItem = await _marketsProductsRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0)
                {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate.Date;
                    item.LastModifiedDate = DateTime.Now;
                    await _marketsProductsRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave()
        {
            return Entry != 0 || Extry != 0 || !string.IsNullOrWhiteSpace(MarketsProductName);
        }

        private MarketsProductsDto BuildFromProperties()
        {
            return new()
            {
                Id = Id,
                MarketsProductName = MarketsProductName,
                Entry = Entry,
                Extry = Extry,
                FirstTermValue = FirstTermValue,
                RealValue = RealValue,
                Note = Note,
                Status=Status,
                CreatedDate= DateTime.Now,
                TheMinimumValue= TheMinimumValue
            };
        }


        public void BuildFromModel(MarketsProductsDto marketsProducts)
        {
            Id = marketsProducts.Id;
            MarketsProductName = marketsProducts.MarketsProductName;
            Entry = marketsProducts.Entry;
            Extry = marketsProducts.Extry;
            FirstTermValue = marketsProducts.FirstTermValue;
            RealValue = marketsProducts.RealValue;
            Note = marketsProducts.Note;
            if (marketsProducts.TheMinimumValue > marketsProducts.RealValue)
            {
                Status = "Visible";
            }
            else
            {
                Status = "Hidden";
            }      
            TheMinimumValue = marketsProducts.TheMinimumValue;
        }

    }
}
