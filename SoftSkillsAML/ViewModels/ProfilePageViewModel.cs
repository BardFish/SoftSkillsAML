using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoftSkillsAML.ViewModels
{
    internal class ProfilePageViewModel : ViewModelBase
    {
        public User User { get; }
        public ObservableCollection<Gender> Genders { get; } = new(MainWindowViewModel.db.Genders.ToList());
        public ObservableCollection<AchievementListItem> Achievements { get; }
        public ObservableCollection<ChartPercentItem> SkillCharts { get; }
        public ObservableCollection<ChartPercentItem> ProgressCharts { get; }

        string _oldPassword = string.Empty;
        string _newPassword = string.Empty;
        public string OldPassword { get => _oldPassword; set => this.RaiseAndSetIfChanged(ref _oldPassword, value); }
        public string NewPassword { get => _newPassword; set => this.RaiseAndSetIfChanged(ref _newPassword, value); }

        Gender? _selectedGender;
        public Gender? SelectedGender { get => _selectedGender; set => this.RaiseAndSetIfChanged(ref _selectedGender, value); }

        public ProfilePageViewModel()
        {
            User = MainWindowViewModel.db.Users.First(x => x.Id == CurrentUserId);
            SelectedGender = Genders.FirstOrDefault(x => x.Id == User.Gender);

            Achievements = new ObservableCollection<AchievementListItem>(MainWindowViewModel.db.UserAchievements.Where(x => x.User == CurrentUserId).OrderBy(x => x.AchievementNavigation.Name).Select(x => new AchievementListItem
            {
                Name = x.AchievementNavigation.Name,
                Description = x.AchievementNavigation.Description,
                Image = x.AchievementNavigation.Image,
                Status = x.IsCompleted ? "Получено" : "Не получено",
                IsCompleted = x.IsCompleted
            }).ToList());

            SkillCharts = new ObservableCollection<ChartPercentItem>(MainWindowViewModel.db.UserSoftSkills.Where(x => x.User == CurrentUserId).Select(x => new ChartPercentItem
            {
                Name = x.SoftSkillNavigation.Name,
                Percent = x.SoftSkillNavigation.MaxPoints == 0 ? 0 : Math.Min(100, (int)Math.Round(x.Points * 100.0 / x.SoftSkillNavigation.MaxPoints)),
            }).ToList());
            foreach (var c in SkillCharts) c.BuildSeries();

            var deps = MainWindowViewModel.db.Departments.Select(d => new { d.Id, d.Name }).ToList();
            ProgressCharts = new ObservableCollection<ChartPercentItem>(deps.Select(d => new ChartPercentItem
            {
                Name = d.Name,
                Percent = GetProgress(d.Id)
            }).ToList());
            foreach (var c in ProgressCharts) c.BuildSeries();
        }

        int GetProgress(int depId)
        {
            var total = MainWindowViewModel.db.Questions.Count(q => q.Department == depId);
            if (total == 0) return 0;
            var done = MainWindowViewModel.db.UserQuestions.Count(x => x.User == CurrentUserId && x.IsAnswered && x.QuestionNavigation.Department == depId);
            return (int)Math.Round(done * 100.0 / total);
        }

        public async void SaveProfile()
        {
            if (SelectedGender == null)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите пол", ButtonEnum.Ok, Icon.Info).ShowAsync(); return;
            }
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (User.Password != MD5.HashData(Encoding.ASCII.GetBytes(OldPassword)))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Старый пароль введен неверно", ButtonEnum.Ok, Icon.Info).ShowAsync(); return;
                }
                User.Password = MD5.HashData(Encoding.ASCII.GetBytes(NewPassword));
                OldPassword = string.Empty; NewPassword = string.Empty;
            }

            User.Gender = SelectedGender.Id;
            MainWindowViewModel.db.SaveChanges();
            await MessageBoxManager.GetMessageBoxStandard("Успех", "Данные сохранены", ButtonEnum.Ok, Icon.Success).ShowAsync();
        }

        public void BackToDepartments() => MainWindowViewModel.Instance.Page = new DepartmentsPageView();
    }

    internal class ChartPercentItem
    {
        public string Name { get; set; } = string.Empty;
        public int Percent { get; set; }
        public ISeries[] Series { get; private set; } = [];
        public void BuildSeries() => Series = [new PieSeries<int> { Values = [Percent], Name = "Процент" }, new PieSeries<int> { Values = [100 - Percent], Name = "Остаток", IsHoverable = false }];
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
