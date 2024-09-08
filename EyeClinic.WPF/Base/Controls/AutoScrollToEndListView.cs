using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EyeClinic.WPF.Base.Controls
{
    public class AutoScrollToEndListView : ListView
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e) {
            base.OnItemsChanged(e);

            var sv = GetChildOfType<ScrollViewer>(this);

            sv?.ScrollToBottom();
            SelectedIndex = Items.Count;
        }

        private static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject {
            if (depObj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
