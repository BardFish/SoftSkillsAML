using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class ProfilePageViewModel : ViewModelBase
    {
        public User User { get; }

        Gender? _selectedGender;
        public Gender? SelectedGender
        {
            get => _selectedGender;
            set => this.RaiseAndSetIfChanged(ref _selectedGender, value);
        }

        public ObservableCollection<Gender> Genders { get; } = new(MainWindowViewModel.db.Genders.ToList());
        public ObservableCollection<UserSoftSkill> Skills { get; }
        public ObservableCollection<AchievementListItem> Achievements { get; }
        public ObservableCollection<DepartmentProgressItem> ProgressByDepartments { get; }

        public ProfilePageViewModel()
        {
            User = MainWindowViewModel.db.Users.First(x => x.Id == CurrentUserId);
            SelectedGender = Genders.FirstOrDefault(x => x.Id == User.Gender);
            Skills = new ObservableCollection<UserSoftSkill>(MainWindowViewModel.db.UserSoftSkills.Where(x => x.User == CurrentUserId).OrderBy(x => x.SoftSkillNavigation.Name).ToList());
            Achievements = new ObservableCollection<AchievementListItem>(MainWindowViewModel.db.UserAchievements.Where(x => x.User == CurrentUserId).OrderBy(x => x.AchievementNavigation.Name).Select(x => new AchievementListItem
            {
                Name = x.AchievementNavigation.Name,
                Description = x.AchievementNavigation.Description,
                Image = x.AchievementNavigation.Image,
                Status = x.IsCompleted ? "Получено" : "Не получено",
                IsCompleted = x.IsCompleted
            }).ToList());

            ProgressByDepartments = new ObservableCollection<DepartmentProgressItem>(
                MainWindowViewModel.db.Departments.Select(x => new DepartmentProgressItem
                {
                    DepartmentName = x.Name,
                    ProgressPercent = GetProgress(CurrentUserId, x.Id)
                }).ToList());
        }

        private static int GetProgress(int userId, int departmentId)
        {
            var total = MainWindowViewModel.db.Questions.Count(q => q.Department == departmentId);
            if (total == 0) return 0;
            var done = MainWindowViewModel.db.UserQuestions.Count(x => x.User == userId && x.QuestionNavigation.Department == departmentId && x.IsAnswered);
            return (int)Math.Round(done * 100.0 / total);
        }

                public async void SaveProfile()
        {
            if (SelectedGender == null)
            {
                var msg = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите пол", ButtonEnum.Ok, Icon.Info);
                await msg.ShowAsync();
                return;
            }

            User.Gender = SelectedGender.Id;
            MainWindowViewModel.db.SaveChanges();
            var ok = MessageBoxManager.GetMessageBoxStandard("Успех", "Данные сохранены", ButtonEnum.Ok, Icon.Success);
            await ok.ShowAsync();
        }

        public void BackToDepartments() => MainWindowViewModel.Instance.Page = new DepartmentsPageView();
    }

    internal class DepartmentProgressItem
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int ProgressPercent { get; set; }
    }
}

namespace SoftSkillsAML.ViewModels
{
    internal class AchievementListItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
