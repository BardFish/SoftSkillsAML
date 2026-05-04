using Avalonia.Controls;
using SoftSkillsAML.ViewModels;

namespace SoftSkillsAML;

public partial class EditDepartmentsPageView : UserControl
{
    public EditDepartmentsPageView()
    {
        InitializeComponent();
        DataContext = new EditDepartmentsPageViewModel();
    }
}
