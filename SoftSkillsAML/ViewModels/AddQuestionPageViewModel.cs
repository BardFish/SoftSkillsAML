using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoftSkillsAML.ViewModels
{
    internal class AddQuestionPageViewModel : ViewModelBase
    {
        public ObservableCollection<Department> Departments { get; } = new(MainWindowViewModel.db.Departments.OrderBy(x => x.Name).ToList());

        Department? _selectedDepartment;
        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set => this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
        }

        string _questionText = string.Empty;
        public string QuestionText
        {
            get => _questionText;
            set => this.RaiseAndSetIfChanged(ref _questionText, value);
        }

        public async void AddQuestion()
        {
            if (SelectedDepartment == null || string.IsNullOrWhiteSpace(QuestionText))
            {
                var err = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните текст и направление", ButtonEnum.Ok, Icon.Info);
                await err.ShowAsync();
                return;
            }

            MainWindowViewModel.db.Questions.Add(new Question
            {
                Text = QuestionText,
                Department = SelectedDepartment.Id,
                HasImage = false
            });
            MainWindowViewModel.db.SaveChanges();

            var ok = MessageBoxManager.GetMessageBoxStandard("Успех", "Задание добавлено", ButtonEnum.Ok, Icon.Success);
            await ok.ShowAsync();
            QuestionText = string.Empty;
        }

        public void BackToStat() => MainWindowViewModel.Instance.Page = new StatisticPageView();
    }
}
