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

            Skills = new ObservableCollection<UserSoftSkill>(
                MainWindowViewModel.db.UserSoftSkills
                    .Where(x => x.User == CurrentUserId)
                    .OrderBy(x => x.SoftSkillNavigation.Name)
                    .ToList());

            Achievements = new ObservableCollection<AchievementListItem>(
                MainWindowViewModel.db.UserAchievements
                    .Where(x => x.User == CurrentUserId)
                    .OrderBy(x => x.AchievementNavigation.Name)
                    .Select(x => new AchievementListItem
                    {
                        Name = x.AchievementNavigation.Name,
                        Description = x.AchievementNavigation.Description,
                        Image = x.AchievementNavigation.Image,
                        Status = x.IsCompleted ? "Получено" : "Не получено",
                        IsCompleted = x.IsCompleted
                    })
                    .ToList());

            var departments = MainWindowViewModel.db.Departments
                .Select(d => new { d.Id, d.Name })
                .ToList();

            var totalByDepartment = MainWindowViewModel.db.Questions
                .GroupBy(q => q.Department)
                .Select(g => new { DepartmentId = g.Key, Total = g.Count() })
                .ToList()
                .ToDictionary(x => x.DepartmentId, x => x.Total);

            var answeredByDepartment = MainWindowViewModel.db.UserQuestions
                .Where(uq => uq.User == CurrentUserId && uq.IsAnswered)
                .Join(
                    MainWindowViewModel.db.Questions,
                    uq => uq.Question,
                    q => q.Id,
                    (uq, q) => q.Department)
                .GroupBy(depId => depId)
                .Select(g => new { DepartmentId = g.Key, Done = g.Count() })
                .ToList()
                .ToDictionary(x => x.DepartmentId, x => x.Done);

            ProgressByDepartments = new ObservableCollection<DepartmentProgressItem>(
                departments.Select(dep =>
                {
                    totalByDepartment.TryGetValue(dep.Id, out var total);
                    answeredByDepartment.TryGetValue(dep.Id, out var done);
                    var percent = total == 0 ? 0 : (int)Math.Round(done * 100.0 / total);

                    return new DepartmentProgressItem
                    {
                        DepartmentName = dep.Name,
                        ProgressPercent = percent
                    };
                }).ToList());
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

    internal class AchievementListItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
