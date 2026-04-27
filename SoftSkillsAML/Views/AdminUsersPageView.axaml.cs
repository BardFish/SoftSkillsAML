using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class AdminUsersPageView : UserControl
{
    public AdminUsersPageView()
    {
        InitializeComponent();
        DataContext = new AdminUsersPageViewModel();
    }
}
