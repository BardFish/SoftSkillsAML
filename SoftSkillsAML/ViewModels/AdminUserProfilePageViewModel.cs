using SoftSkillsAML.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class AdminUserProfilePageViewModel : ViewModelBase
    {
        public User User { get; }
        public string GenderName { get; }
        public ObservableCollection<ChartPercentItem> SkillCharts { get; }
        public ObservableCollection<ChartPercentItem> ProgressCharts { get; }
        public ObservableCollection<AchievementListItem> Achievements { get; }

        public AdminUserProfilePageViewModel(int userId)
        {
            User = MainWindowViewModel.db.Users.First(x => x.Id == userId);
            GenderName = MainWindowViewModel.db.Genders.First(x => x.Id == User.Gender).Name;

            SkillCharts = new ObservableCollection<ChartPercentItem>(MainWindowViewModel.db.UserSoftSkills.Where(x => x.User == userId).Select(x => new ChartPercentItem
            {
                Name = x.SoftSkillNavigation.Name,
                Percent = x.SoftSkillNavigation.MaxPoints == 0 ? 0 : Math.Min(100, (int)Math.Round(x.Points * 100.0 / x.SoftSkillNavigation.MaxPoints))
            }).ToList());
            foreach (var c in SkillCharts) c.BuildSeries();

            var deps = MainWindowViewModel.db.Departments.Select(d => new { d.Id, d.Name }).ToList();
            ProgressCharts = new ObservableCollection<ChartPercentItem>(deps.Select(d => new ChartPercentItem { Name = d.Name, Percent = GetProgress(userId, d.Id) }).ToList());
            foreach (var c in ProgressCharts) c.BuildSeries();

            Achievements = new ObservableCollection<AchievementListItem>(MainWindowViewModel.db.UserAchievements.Where(x => x.User == userId).Select(x => new AchievementListItem
            {
                Name = x.AchievementNavigation.Name,
                Description = x.AchievementNavigation.Description,
                Image = x.AchievementNavigation.Image,
                Status = x.IsCompleted ? "Получено" : "Не получено",
                IsCompleted = x.IsCompleted
            }).ToList());
        }

        int GetProgress(int userId, int depId)
        {
            var total = MainWindowViewModel.db.Questions.Count(q => q.Department == depId);
            if (total == 0) return 0;
            var done = MainWindowViewModel.db.UserQuestions.Count(x => x.User == userId && x.IsAnswered && x.QuestionNavigation.Department == depId);
            return (int)Math.Round(done * 100.0 / total);
        }

        public void BackToUsers() => MainWindowViewModel.Instance.Page = new AdminUsersPageView();
    }
}
