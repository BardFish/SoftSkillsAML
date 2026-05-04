using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class QuestionsPageView : UserControl
{
    public QuestionsPageView(int departmentId)
    {
        InitializeComponent();
        DataContext = new QuestionsPageViewModel(departmentId);
    }
}
