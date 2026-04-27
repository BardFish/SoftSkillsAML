using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using SoftSkillsAML.Models;

namespace SoftSkillsAML.ViewModels
{
    internal class AddAchievementPageViewModel : ViewModelBase
    {
        string _name = string.Empty;
        string _description = string.Empty;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public async void AddAchievement()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
            {
                var err = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните название и описание", ButtonEnum.Ok, Icon.Info);
                await err.ShowAsync();
                return;
            }

            MainWindowViewModel.db.Achievements.Add(new Achievement
            {
                Name = Name,
                Description = Description,
                Image = []
            });
            MainWindowViewModel.db.SaveChanges();

            var ok = MessageBoxManager.GetMessageBoxStandard("Успех", "Достижение добавлено", ButtonEnum.Ok, Icon.Success);
            await ok.ShowAsync();
            Name = string.Empty;
            Description = string.Empty;
        }

        public void BackToStat() => MainWindowViewModel.Instance.Page = new StatisticPageView();
    }
}
