using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class DepartmentsPageViewModel: ViewModelBase
    {
        public ObservableCollection<Department> Departments { get; } = new(MainWindowViewModel.db.Departments.OrderBy(x => x.Name).ToList());

        Department? _selectedDepartment;
        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set => this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
        }

        public void OpenDepartment()
        {
            if (SelectedDepartment == null) return;
            MainWindowViewModel.Instance.Page = new QuestionsPageView(SelectedDepartment.Id);
        }

        public void OpenProfile()
        {
            MainWindowViewModel.Instance.Page = new ProfilePageView();
        }

        public void Logout()
        {
            CurrentUserId = 0;
            MainWindowViewModel.Instance.Page = new AuthPageView();
        }
    }
}
