using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class AdminUserProfilePageView : UserControl
{
    public AdminUserProfilePageView(int userId)
    {
        InitializeComponent();
        DataContext = new AdminUserProfilePageViewModel(userId);
    }
}
