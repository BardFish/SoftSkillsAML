using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class EditDepartmentsPageViewModel : ViewModelBase
    {
        public ObservableCollection<Department> Departments { get; } = new(MainWindowViewModel.db.Departments.OrderBy(x => x.Name).ToList());

        Department? _selectedDepartment;
        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set => this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
        }

        public async void Save()
        {
            if (SelectedDepartment == null)
            {
                await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите направление", ButtonEnum.Ok, Icon.Info).ShowAsync();
                return;
            }

            MainWindowViewModel.db.SaveChanges();
            await MessageBoxManager.GetMessageBoxStandard("Успех", "Изменения сохранены", ButtonEnum.Ok, Icon.Success).ShowAsync();
        }

        public void Back() => MainWindowViewModel.Instance.Page = new StatisticPageView();
    }
}
