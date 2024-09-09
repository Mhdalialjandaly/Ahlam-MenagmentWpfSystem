using Tulpep.NotificationWindow;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.PDFMerger
{
   public class Notifying
    {
        public void ShowNotificationPopup(string titl, string content, bool rtf)
        {
            var popupNotifier = new PopupNotifier();

            // Customize the popup properties
            popupNotifier.TitleText = titl;
            popupNotifier.ContentText = content;
            popupNotifier.IsRightToLeft = rtf; // Set to true if your language is right-to-left

            // Display the popup
            popupNotifier.Popup();
        }
    }
}
