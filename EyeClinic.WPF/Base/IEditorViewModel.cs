using System.Threading.Tasks;

namespace EyeClinic.WPF.Base
{
    public interface IEditorViewModel<TModel>
    {
        Task<TModel> Save();
        TModel BuildFromProperties();
        bool ValidForSave();
        void BuildFromModel(TModel selectedContact);
    }
}
