using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class AuthPageView : UserControl
{
    public AuthPageView()
    {
        InitializeComponent();
        DataContext = new AuthPageViewModel();
    }
}