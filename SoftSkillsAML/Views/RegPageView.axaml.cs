using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class RegPageView : UserControl
{
    public RegPageView()
    {
        InitializeComponent();
        DataContext = new RegPageViewModel();
    }
}