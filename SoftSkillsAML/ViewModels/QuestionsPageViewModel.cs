using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class QuestionsPageViewModel : ViewModelBase
    {
        public int DepartmentId { get; }
        public string DepartmentName { get; }

        public ObservableCollection<UserQuestion> Questions { get; }

        UserQuestion? _selectedQuestion;
        public UserQuestion? SelectedQuestion
        {
            get => _selectedQuestion;
            set => this.RaiseAndSetIfChanged(ref _selectedQuestion, value);
        }

        public QuestionsPageViewModel(int departmentId)
        {
            DepartmentId = departmentId;
            DepartmentName = MainWindowViewModel.db.Departments.First(x => x.Id == DepartmentId).Name;
            Questions = new ObservableCollection<UserQuestion>(
                MainWindowViewModel.db.UserQuestions
                .Where(x => x.User == CurrentUserId && x.QuestionNavigation.Department == departmentId)
                .OrderBy(x => x.QuestionNavigation.Id)
                .ToList());
        }

        public string QuestionStatus(UserQuestion uq) => uq.IsAnswered ? "Пройдено" : "Не пройдено";

        public void OpenQuestion()
        {
            if (SelectedQuestion == null) return;
            MainWindowViewModel.Instance.Page = new QuestionDetailsPageView(SelectedQuestion.Question);
        }

        public void BackToDepartments() => MainWindowViewModel.Instance.Page = new DepartmentsPageView();
    }
}
