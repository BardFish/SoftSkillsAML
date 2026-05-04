using ReactiveUI;
using SoftSkillsAML.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class AdminUsersPageViewModel : ViewModelBase
    {
        string _minAge = string.Empty;
        string _minProgress = string.Empty;

        public string MinAge
        {
            get => _minAge;
            set => this.RaiseAndSetIfChanged(ref _minAge, value);
        }

        public string MinProgress
        {
            get => _minProgress;
            set => this.RaiseAndSetIfChanged(ref _minProgress, value);
        }

        public ObservableCollection<AdminUserListItem> Users { get; } = new();

        AdminUserListItem? _selectedUser;
        public AdminUserListItem? SelectedUser
        {
            get => _selectedUser;
            set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
        }

        public AdminUsersPageViewModel()
        {
            ApplyFilters();
        }

        public void ApplyFilters()
        {
            int age = int.TryParse(MinAge, out var parsedAge) ? parsedAge : 0;
            int progress = int.TryParse(MinProgress, out var parsedProgress) ? parsedProgress : 0;

            var users = MainWindowViewModel.db.Users.Where(x => !x.IsAdmin).ToList()
                .Select(x => new AdminUserListItem
                {
                    Id = x.Id,
                    Login = x.Login,
                    Age = GetAge(x.Birthday),
                    Progress = GetOverallProgress(x.Id),
                    IsBlocked = x.IsBlocked
                })
                .Where(x => x.Age >= age && x.Progress >= progress)
                .OrderBy(x => x.Login)
                .ToList();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private static int GetAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            if (birthday.Date > today.AddYears(-age)) age--;
            return Math.Clamp(age, 0, 100);
        }

        private static int GetOverallProgress(int userId)
        {
            var total = MainWindowViewModel.db.Questions.Count();
            if (total == 0) return 0;
            var done = MainWindowViewModel.db.UserQuestions.Count(x => x.User == userId && x.IsAnswered);
            return (int)Math.Round(done * 100.0 / total);
        }

        public void ToggleBlock()
        {
            if (SelectedUser == null) return;
            var user = MainWindowViewModel.db.Users.First(x => x.Id == SelectedUser.Id);
            user.IsBlocked = !user.IsBlocked;
            MainWindowViewModel.db.SaveChanges();
            ApplyFilters();
        }

        public void OpenProfile()
        {
            if (SelectedUser == null) return;
            MainWindowViewModel.Instance.Page = new AdminUserProfilePageView(SelectedUser.Id);
        }

        public void BackToStat() => MainWindowViewModel.Instance.Page = new StatisticPageView();
    }

    internal class AdminUserListItem
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public int Age { get; set; }
        public int Progress { get; set; }
        public bool IsBlocked { get; set; }
    }
}
