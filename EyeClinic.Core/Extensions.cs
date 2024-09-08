using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EyeClinic.Core
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty(this ICollection list) {
            return list == null || list.Count == 0;
        }

        public static void UpdateItem<T>(this ObservableCollection<T> collection, T oldItem, T newItem) {
            var index = collection.IndexOf(oldItem);
            collection.RemoveAt(index);
            collection.Insert(index, newItem);
        }

        public static void UpdateItem<T>(this List<T> collection, T oldItem, T newItem) {
            var index = collection.IndexOf(oldItem);
            collection.RemoveAt(index);
            collection.Insert(index, newItem);
        }
    }
}
