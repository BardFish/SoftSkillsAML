using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class StatisticPageViewModel: ViewModelBase
    {
        public ObservableCollection<ChartPercentItem> DepartmentConversion { get; }
        public ObservableCollection<ChartPercentItem> SkillsAverages { get; }

        public StatisticPageViewModel()
        {
            var departments = MainWindowViewModel.db.Departments.ToList();
            var users = MainWindowViewModel.db.Users.Where(u => !u.IsAdmin).Select(u => u.Id).ToList();
            var questions = MainWindowViewModel.db.Questions.Select(q => new { q.Id, q.Department }).ToList();
            var answered = MainWindowViewModel.db.UserQuestions.Where(uq => uq.IsAnswered).Select(uq => new { uq.User, uq.Question }).ToList();

            DepartmentConversion = new ObservableCollection<ChartPercentItem>(departments.Select(d =>
            {
                var departmentQuestionIds = questions.Where(q => q.Department == d.Id).Select(q => q.Id).ToList();
                var started = users.Count(userId => answered.Any(a => a.User == userId && departmentQuestionIds.Contains(a.Question)));
                var completed = users.Count(userId => departmentQuestionIds.Count > 0 && departmentQuestionIds.All(questionId => answered.Any(a => a.User == userId && a.Question == questionId)));
                var percent = started == 0 ? 0 : (int)System.Math.Round(completed * 100.0 / started);
                return new ChartPercentItem { Name = d.Name, Percent = percent };
            }).ToList());
            foreach (var c in DepartmentConversion) c.BuildSeries();

            SkillsAverages = new ObservableCollection<ChartPercentItem>(MainWindowViewModel.db.SoftSkills.ToList().Select(s =>
            {
                var avg = MainWindowViewModel.db.UserSoftSkills.Where(us => us.SoftSkill == s.Id && !us.UserNavigation.IsAdmin).Select(us => (double?)us.Points).Average() ?? 0;
                var percent = s.MaxPoints == 0 ? 0 : (int)System.Math.Round(avg * 100.0 / s.MaxPoints);
                return new ChartPercentItem { Name = s.Name, Percent = percent };
            }).ToList());
            foreach (var c in SkillsAverages) c.BuildSeries();
        }

        public void OpenUsers() => MainWindowViewModel.Instance.Page = new AdminUsersPageView();
        public void OpenAddQuestion() => MainWindowViewModel.Instance.Page = new AddQuestionPageView();
        public void OpenAddAchievement() => MainWindowViewModel.Instance.Page = new AddAchievementPageView();
        public void Logout() { CurrentUserId = 0; MainWindowViewModel.Instance.Page = new AuthPageView(); }
    }
}
