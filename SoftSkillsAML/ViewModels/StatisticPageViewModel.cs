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
            DepartmentConversion = new ObservableCollection<DepartmentConversionItem>(
                MainWindowViewModel.db.Departments.Select(d => new DepartmentConversionItem
                {
                    DepartmentName = d.Name,
                    Started = MainWindowViewModel.db.Users.Count(u => !u.IsAdmin && MainWindowViewModel.db.UserQuestions.Any(uq => uq.User == u.Id && uq.QuestionNavigation.Department == d.Id && uq.IsAnswered)),
                    Completed = MainWindowViewModel.db.Users.Count(u => !u.IsAdmin && IsDepartmentCompleted(u.Id, d.Id))
                }).ToList());

            SkillsAverages = new ObservableCollection<SkillAverageItem>(
                MainWindowViewModel.db.SoftSkills.Select(s => new SkillAverageItem
                {
                    SkillName = s.Name,
                    Average = MainWindowViewModel.db.UserSoftSkills.Where(us => us.SoftSkill == s.Id).Select(us => (double?)us.Points).Average() ?? 0
                }).ToList());
        }

        private static bool IsDepartmentCompleted(int userId, int depId)
        {
            var total = MainWindowViewModel.db.Questions.Count(q => q.Department == depId);
            if (total == 0) return false;
            var done = MainWindowViewModel.db.UserQuestions.Count(uq => uq.User == userId && uq.QuestionNavigation.Department == depId && uq.IsAnswered);
            return done == total;
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
