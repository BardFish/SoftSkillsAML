using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class AdminUserProfilePageViewModel : ViewModelBase
    {
        public User User { get; }
        public ObservableCollection<UserSoftSkill> Skills { get; }

        public AdminUserProfilePageViewModel(int userId)
        {
            User = MainWindowViewModel.db.Users.First(x => x.Id == userId);
            Skills = new ObservableCollection<UserSoftSkill>(MainWindowViewModel.db.UserSoftSkills.Where(x => x.User == userId).OrderBy(x => x.SoftSkillNavigation.Name).ToList());
        }

        public void BackToUsers() => MainWindowViewModel.Instance.Page = new AdminUsersPageView();
    }
}
