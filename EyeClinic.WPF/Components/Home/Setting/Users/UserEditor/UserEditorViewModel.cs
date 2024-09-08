using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Setting.Users.UserEditor
{
    public partial class UserEditorViewModel
    {
        public UserEditorViewModel() { }
    }

    public partial class UserEditorViewModel : BaseViewModel<UserEditorView>
    {
        private readonly IUserRepository _userRepository;

        public UserEditorViewModel(IUserRepository userRepository) {
            _userRepository = userRepository;
        }


        public override async Task Initialize() {
            Roles = new ObservableCollection<RoleDto>(
                await _userRepository.GetRoles());
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }


        public ObservableCollection<RoleDto> Roles { get; set; }
        public RoleDto SelectedRole { get; set; }

        public async Task<Model.UserDto> Save() {
            if (ValidForSave()) {
                var item = BuildFromProperties();
                if (Operation == operation.Add) {
                    var addedItem = await _userRepository.Add(item);
                    return addedItem;
                }

                if (Operation == operation.Edit && Id > 0) {
                    item.Id = Id;
                    item.CreatedDate = CreatedDate;
                    item.LastModifiedDate = DateTime.Now;
                    await _userRepository.Update(item);
                    return item;
                }
            }

            return null;
        }

        private bool ValidForSave() {
            return !string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(Password)
                && SelectedRole != null;
        }

        private Model.UserDto BuildFromProperties() {
            return new() {
                Id = Id,
                UserName = UserName,
                Password = Password,
                IsActive = IsActive,
                RoleId = SelectedRole.Id,
                CreatedDate = CreatedDate
            };
        }

        public void BuildFromModel(UserDto disease) {
            Id = disease.Id;
            UserName = disease.UserName;
            Password = disease.Password;
            IsActive = disease.IsActive;
            RoleId = disease.RoleId;
            CreatedDate = disease.CreatedDate;

            SelectedRole = Roles.FirstOrDefault(e => e.Id == disease.RoleId);
        }
    }
}
