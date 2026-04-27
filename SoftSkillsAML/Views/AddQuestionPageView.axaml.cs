using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class AddQuestionPageView : UserControl
{
    public AddQuestionPageView()
    {
        InitializeComponent();
        DataContext = new AddQuestionPageViewModel();
    }
}
