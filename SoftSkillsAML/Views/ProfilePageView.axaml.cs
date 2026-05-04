using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class ProfilePageView : UserControl
{
    public ProfilePageView()
    {
        InitializeComponent();
        DataContext = new ProfilePageViewModel();
    }
}
