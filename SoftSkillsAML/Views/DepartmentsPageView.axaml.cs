using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class DepartmentsPageView : UserControl
{
    public DepartmentsPageView()
    {
        InitializeComponent();
        DataContext = new DepartmentsPageViewModel();
    }
}