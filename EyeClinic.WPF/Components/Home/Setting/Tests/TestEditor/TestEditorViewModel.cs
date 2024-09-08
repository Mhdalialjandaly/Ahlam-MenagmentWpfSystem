using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shapes;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.Setting.Tests.TestEditor
{
    public partial class TestEditorViewModel
    {
        internal Action<object, object> OnSave;

        public TestEditorViewModel() { }
    }

    public partial class TestEditorViewModel : BaseViewModel<TestEditorView>
    {
        private readonly IUnityContainer _container;
        private readonly ITestsRepository _testsRepository;
        private readonly INotificationService _notificationService;

        public TestEditorViewModel(ITestsRepository testsRepository,IUnityContainer unityContainer,INotificationService notificationService) {
            _testsRepository = testsRepository;
            _container = unityContainer;
            _notificationService = notificationService;

            AddImageCommand1 = new RelayCommand(GetImageFromPath);
            AddImageCommand2 = new RelayCommand(GetImageFromPath2);
            //AddImageCommand3 = new RelayCommand(GetImageFromPath3);

            AddPdfFileCommand = new RelayCommand(AddPdfFile);
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public string Unit { get; set; }
        public double Quintity { get; set; }
        public string FirstTermBalance { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string ImagePath { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public byte Imagex { get; set; }
        public byte[] Imagex2 { get; set; }
        public byte[] Imagex3 { get; set; }
        public byte[] Imagex4 { get; set; }
        public byte[] PDfFile { get; set; }
        public byte[] PDfFile1 { get; set; }
        public byte[] PDfFile2 { get; set; }
        public Image image1 { get; set; }
        public Image image2 { get; set; }
        public Image image3 { get; set; }
        public string path1 { get; set; }
        public string path2 { get; set; }
        public string path3 { get; set; }        
        public  ICommand AddImageCommand1 { set; get; }
        public  ICommand AddImageCommand2 { set; get; }
        public ICommand AddImageCommand3 { set; get; }
        public ICommand AddPdfFileCommand { set; get; }

        public async Task<Model.TestDto> Save() {
          
                if (ValidForSave())
                {
                    var item = BuildFromProperties();
                    if (Operation == operation.Add)
                    {
                        var addedItem =
                            await _testsRepository.Add(item);
                        _notificationService.Success("Saved .");
                        return addedItem;

                    }

                    if (Operation == operation.Edit && Id > 0)
                    {
                        item.Id = Id;
                        item.CreatedDate = CreatedDate;
                        item.LastModifiedDate = DateTime.Now;
                        await _testsRepository.Update(item);
                        _notificationService.Success("Updated .");

                        return item;
                    }
                }

                return null;
            
            

        }

        private bool ValidForSave() {
            
          
                return !string.IsNullOrWhiteSpace(EnName) && !string.IsNullOrWhiteSpace(ArName)
               && !string.IsNullOrWhiteSpace(Code) ;           
           
            
        }

        private Model.TestDto BuildFromProperties() {
            //if (image1!= null)
            //{

            //    //Imagex2 = ImageToByteArray(image1);
            //}
            //// Replace 'yourImage' with your actual image
            if (path2 != null)
                ImagePath2 = path2; // Replace 'yourImage' with your actual image
            //if (image1 != null)
            //{
            //    Imagex4 = ImageToByteArray(image3);
            //} // Replace 'yourImage' with your actual image
            if (path1 != null)            
                ImagePath = path1;
          
            return new() {
                Id = Id,
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = CreatedDate,                
                ImagePath2 = ImagePath2,                
                Unit = Unit,
                Quintity = Quintity,
                FirstTermBalance = FirstTermBalance,
                IsProduct = false,
                ImagePath = ImagePath
            };
        }

        public void BuildFromModel(Model.TestDto disease) {
            Id = disease.Id;
            Code = disease.Code;
            ArName = disease.ArName;
            EnName = disease.EnName;
            CreatedDate = disease.CreatedDate;           
            ImagePath2 =disease.ImagePath2;            
            Unit = disease.Unit;
            Quintity = disease.Quintity;
            FirstTermBalance = disease.FirstTermBalance;
            ImagePath =disease.ImagePath;
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
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                // Use 'selectedFilePath' as needed (e.g., load the image)
                path1 = selectedFilePath;
                //image1 = Image.FromFile(selectedFilePath);
                _notificationService.Success("This Pdf 1 Has Added");
            }

        }
        public void GetImageFromPath2()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                // Use 'selectedFilePath' as needed (e.g., load the image)
                path2 = selectedFilePath;
                //image2 = Image.FromFile(selectedFilePath);
                _notificationService.Success("This Image 1 Has Added");
            }

        }
        public void GetImageFromPath3()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF3 Files (*.pdf)|*.pdf;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                // Use 'selectedFilePath' as needed (e.g., load the image)
                path3 = selectedFilePath;
                //image1 = Image.FromFile(selectedFilePath);
                _notificationService.Success("This pdf 3 Has Added");
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
        public byte[] ConvertPdfToArray(string path)
        {
            byte[] bytes;
            string pdfFilePath = path;
           return bytes = System.IO.File.ReadAllBytes(path);
        }
    }
}
