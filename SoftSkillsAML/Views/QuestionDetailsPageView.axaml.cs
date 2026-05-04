using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class QuestionDetailsPageView : UserControl
{
    public QuestionDetailsPageView(int questionId)
    {
        InitializeComponent();
        DataContext = new QuestionDetailsPageViewModel(questionId);
    }
}
