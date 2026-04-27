using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class AddAchievementPageView : UserControl
{
    public AddAchievementPageView()
    {
        InitializeComponent();
        DataContext = new AddAchievementPageViewModel();
    }
}
