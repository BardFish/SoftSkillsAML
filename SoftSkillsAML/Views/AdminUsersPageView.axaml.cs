using Avalonia.Controls;
using Avalonia.Interactivity;
using SoftSkillsAML.ViewModels;
using System.Linq;

namespace SoftSkillsAML;

public partial class AdminUsersPageView : UserControl
{
    public AdminUsersPageView()
    {
        InitializeComponent();
        DataContext = new AdminUsersPageViewModel();
    }

    private void BlockedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox { DataContext: AdminUserListItem userItem } checkBox) return;

        var user = MainWindowViewModel.db.Users.FirstOrDefault(x => x.Id == userItem.Id);
        if (user == null) return;

        user.IsBlocked = checkBox.IsChecked == true;
        userItem.IsBlocked = user.IsBlocked;
        MainWindowViewModel.db.SaveChanges();
    }
}
