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

        public ObservableCollection<QuestionListItem> Questions { get; }

        QuestionListItem? _selectedQuestion;
        public QuestionListItem? SelectedQuestion
        {
            get => _selectedQuestion;
            set => this.RaiseAndSetIfChanged(ref _selectedQuestion, value);
        }

        public QuestionsPageViewModel(int departmentId)
        {
            DepartmentId = departmentId;
            DepartmentName = MainWindowViewModel.db.Departments.First(x => x.Id == DepartmentId).Name;
            Questions = new ObservableCollection<QuestionListItem>(
                MainWindowViewModel.db.UserQuestions
                .Where(x => x.User == CurrentUserId && x.QuestionNavigation.Department == departmentId)
                .OrderBy(x => x.QuestionNavigation.Id)
                .ToList()
                .Select(x => new QuestionListItem
                {
                    QuestionId = x.Question,
                    IsAnswered = x.IsAnswered,
                    Number = x.QuestionNavigation.NumberInDepartment.ToString()
                }));
        }

        public void OpenQuestion()
        {
            if (SelectedQuestion == null) return;
            MainWindowViewModel.Instance.Page = new QuestionDetailsPageView(SelectedQuestion.QuestionId);
        }

        public void BackToDepartments() => MainWindowViewModel.Instance.Page = new DepartmentsPageView();
    }

    internal class QuestionListItem
    {
        public int QuestionId { get; set; }
        public string Number { get; set; } = string.Empty;
        public bool IsAnswered { get; set; }
    }
}
