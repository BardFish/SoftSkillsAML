using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class StatisticPageViewModel: ViewModelBase
    {
        public ObservableCollection<DepartmentConversionItem> DepartmentConversion { get; }
        public ObservableCollection<SkillAverageItem> SkillsAverages { get; }

        public StatisticPageViewModel()
        {
            var departments = MainWindowViewModel.db.Departments.ToList();
            var users = MainWindowViewModel.db.Users.Where(u => !u.IsAdmin).Select(u => u.Id).ToList();
            var questions = MainWindowViewModel.db.Questions.Select(q => new { q.Id, q.Department }).ToList();
            var answered = MainWindowViewModel.db.UserQuestions
                .Where(uq => uq.IsAnswered)
                .Select(uq => new { uq.User, uq.Question })
                .ToList();

            DepartmentConversion = new ObservableCollection<DepartmentConversionItem>(
                departments.Select(d =>
                {
                    var departmentQuestionIds = questions.Where(q => q.Department == d.Id).Select(q => q.Id).ToList();

                    var started = users.Count(userId => answered.Any(a => a.User == userId && departmentQuestionIds.Contains(a.Question)));

                    var completed = users.Count(userId =>
                        departmentQuestionIds.Count > 0 &&
                        departmentQuestionIds.All(questionId => answered.Any(a => a.User == userId && a.Question == questionId)));

                    return new DepartmentConversionItem
                    {
                        DepartmentName = d.Name,
                        Started = started,
                        Completed = completed
                    };
                }).ToList());

            SkillsAverages = new ObservableCollection<SkillAverageItem>(
                MainWindowViewModel.db.SoftSkills.Select(s => new SkillAverageItem
                {
                    SkillName = s.Name,
                    Average = MainWindowViewModel.db.UserSoftSkills.Where(us => us.SoftSkill == s.Id).Select(us => (double?)us.Points).Average() ?? 0
                }).ToList());
        }

        public void OpenUsers() => MainWindowViewModel.Instance.Page = new AdminUsersPageView();
        public void OpenAddQuestion() => MainWindowViewModel.Instance.Page = new AddQuestionPageView();
        public void OpenAddAchievement() => MainWindowViewModel.Instance.Page = new AddAchievementPageView();
        public void Logout()
        {
            CurrentUserId = 0;
            MainWindowViewModel.Instance.Page = new AuthPageView();
        }
    }

    internal class DepartmentConversionItem
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int Started { get; set; }
        public int Completed { get; set; }
    }

    internal class SkillAverageItem
    {
        public string SkillName { get; set; } = string.Empty;
        public double Average { get; set; }
    }
}
