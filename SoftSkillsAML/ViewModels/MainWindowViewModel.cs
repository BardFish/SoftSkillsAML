using Avalonia.Controls;
using ReactiveUI;
using SoftSkillsAML.Models;

namespace SoftSkillsAML.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static _43pBardakovUpContext db = new _43pBardakovUpContext();

        public static MainWindowViewModel Instance;

        public MainWindowViewModel()
        {
            Instance = this;
        }

        UserControl _page = new AuthPageView();

        public UserControl Page
        {
            get => _page;
            set => this.RaiseAndSetIfChanged(ref _page, value);
        }
    }
}
