using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class StatisticPageView : UserControl
{
    public StatisticPageView()
    {
        InitializeComponent();
        DataContext = new StatisticPageViewModel();
    }
}