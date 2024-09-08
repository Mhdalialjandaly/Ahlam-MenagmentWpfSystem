using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Tests.TestEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddItemsViewModel
{
    
        public partial class AddItemsViewModel
        {
            public AddItemsViewModel() { }
        }

        public partial class AddItemsViewModel : BaseViewModel<AddItemsView>
        {
            
            private readonly ITestsRepository _testsRepository;
            private readonly INotificationService _notificationService;
            private readonly IReadyProductRepository _readyProductRepository;
            public AddItemsViewModel(ITestsRepository testsRepository, INotificationService notificationService,IReadyProductRepository readyProductRepository)
            {
                _testsRepository = testsRepository;              
                _notificationService = notificationService;
                _readyProductRepository = readyProductRepository;

                AddImageCommand1 = new RelayCommand(GetImageFromPath);
                AddImageCommand2 = new RelayCommand(GetImageFromPath2);
                AddImageCommand3 = new RelayCommand(GetImageFromPath3);

                AddPdfFileCommand = new RelayCommand(AddPdfFile);

              
                UnitItem = new List<string> {"غ","كغ","ل","مل","قطعة"};
                DateOf = DateTime.Now;
            }

            public int Id { get; set; }
            public DateTime DateOf { get; set; }
            public string Code { get; set; }
            public string EnName { get; set; }
            public string ArName { get; set; }
            public string Unit { get; set; }
            public double UnitValue { get; set; }
            public string SelectedUnit { set; get; }
            public List<string> UnitItem { get; set; }
            public double Quintity { get; set; }

            public string FirstTermBalance { get; set; }

            public double FirstTermBalanceWareHouseValue { get; set; }
            public double TotalWasteWareHouseValue { get; set; }
            public double TotalWareHouseValue { get; set; }
            public double TotalEntryWareHouseValue { get; set; }

            public DateTime CreatedDate { get; set; }
            public string ImagePath { get; set; }
            public string ImagePath2 { get; set; }
            public string ImagePath3 { get; set; }
            public byte Imagex { get; set; }
            public byte[] Imagex2 { get; set; }
            public byte[] Imagex3 { get; set; }
            public byte[] Imagex4 { get; set; }
            public byte[] PDfFile { get; set; }
            public Image image1 { get; set; }
            public Image image2 { get; set; }
            public Image image3 { get; set; }
            public double TotalWaste { get; set; }
            public double TotalWight { get; set; }
            public double TotalValue { get; set; }
            public double TotalResult    { get; set; }
            public ICommand AddImageCommand1 { set; get; }
            public ICommand AddImageCommand2 { set; get; }
            public ICommand AddImageCommand3 { set; get; }
            public ICommand AddPdfFileCommand { set; get; }

            public async Task<Model.TestDto> Save()
            {
                if (ValidForSave())
                {
                    ConvertUnits();
                    var item = BuildFromProperties();
                    if (Operation == operation.Add)
                    {
                        var addedItem = await _testsRepository.Add(item);
                        _notificationService.Success("Saved .");
                        return addedItem;
                    }

                    if (Operation == operation.Edit && Id > 0)
                    {
                        item.Id = Id;
                        item.CreatedDate = DateOf;
                        item.LastModifiedDate = DateTime.Now;
                        await _testsRepository.Update(item);
                        _notificationService.Success("Updated .");

                        return item;
                    }
                }

                return null;

            }

            private bool ValidForSave()
            {
           

            return !string.IsNullOrWhiteSpace(EnName) && !string.IsNullOrWhiteSpace(ArName)
               && !string.IsNullOrWhiteSpace(Code);


            }
        public void ConvertUnits() 
        {
            switch (SelectedUnit)
            {
                case "غ":
                    UnitValue = UnitValue / 1000;
                    SelectedUnit = "كغ";
                    break;
                case "مل":
                    UnitValue = UnitValue / 1000;
                    SelectedUnit = "ل";
                    break;
                case "قطعة":
                    UnitValue = UnitValue ;
                    SelectedUnit = "قطعة";
                    break;
            }
        }

            private Model.TestDto BuildFromProperties()
            {
                if (image1 != null)
                {
                    Imagex2 = ImageToByteArray(image1);
                }
                // Replace 'yourImage' with your actual image
                if (image1 != null)
                {
                    Imagex3 = ImageToByteArray(image2);
                } // Replace 'yourImage' with your actual image
                if (image1 != null)
                {
                    Imagex4 = ImageToByteArray(image3);
                } // Replace 'yourImage' with your actual image
                if (string.IsNullOrWhiteSpace(FirstTermBalance))
                {
                   FirstTermBalance= "0";
                }
            if (string.IsNullOrWhiteSpace(SelectedUnit))
            {
                SelectedUnit = "كغ";
            }
                return new()
                {
                    Id = Id,
                    Code = Code,
                    ArName = ArName,
                    EnName = EnName,
                    CreatedDate = DateOf,
                    Imagex2 = Imagex2,
                    Imagex3 = Imagex3,
                    Imagex4 = Imagex4,
                    Unit = SelectedUnit,
                    Quintity = Quintity,
                    FirstTermBalance = FirstTermBalance,
                    UnitValue = UnitValue,
                    TotalResult = UnitValue + Convert.ToDouble(FirstTermBalance),
                    IsProduct = true                    
                };
            }

            public void BuildFromModel(Model.TestDto disease)
            {
                Id = disease.Id;
                Code = disease.Code;
                ArName = disease.ArName;
                EnName = disease.EnName;
                DateOf = disease.CreatedDate;
                Imagex2 = disease.Imagex2;
                Imagex3 = disease.Imagex3;
                Imagex4 = disease.Imagex4;
                UnitValue = disease.UnitValue;
                Quintity = disease.Quintity;
                FirstTermBalance = disease.FirstTermBalance;
                SelectedUnit = disease.Unit;                
            }
            public byte[] ImageToByteArray(System.Drawing.Image imageIn)
            {
                using (var ms = new MemoryStream())
                {
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    return ms.ToArray();
                }
            }
            public void GetImageFromPath()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    // Use 'selectedFilePath' as needed (e.g., load the image)
                    ImagePath = selectedFilePath;
                    image1 = Image.FromFile(selectedFilePath);
                    _notificationService.Success("This Image 1 Has Added");
                }

            }
            public void GetImageFromPath2()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    // Use 'selectedFilePath' as needed (e.g., load the image)
                    ImagePath2 = selectedFilePath;
                    image2 = Image.FromFile(selectedFilePath);
                    _notificationService.Success("This Image 2 Has Added");

                }

            }
            public void GetImageFromPath3()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    // Use 'selectedFilePath' as needed (e.g., load the image)
                    ImagePath3 = selectedFilePath;
                    image3 = Image.FromFile(selectedFilePath);
                    _notificationService.Success("This Image 3 Has Added");

                }

            }
            int counter = 0;
            public void AddPdfFile()
            {

                if (counter == 0)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Pdf File (*.pdf, *.pdf)|*.pdf;*.pdf";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PDfFile = File.ReadAllBytes(openFileDialog.FileName);
                    }
                    counter++;
                }
                else
                {
                    using (var folderDialog = new FolderBrowserDialog())
                    {
                        // Set initial directory (optional)
                        folderDialog.SelectedPath = @"C:\";

                        // Show the dialog and get the result
                        DialogResult result = folderDialog.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            // Get the selected folder path
                            string selectedPath = folderDialog.SelectedPath;
                            File.WriteAllBytesAsync(selectedPath, PDfFile);
                        }
                    }

                }

            }
        }
    
}
