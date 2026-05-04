using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SoftSkillsAML.ViewModels;
using System.IO;
using System.Linq;

namespace SoftSkillsAML;

public partial class EditDepartmentsPageView : UserControl
{
    public EditDepartmentsPageView()
    {
        InitializeComponent();
        DataContext = new EditDepartmentsPageViewModel();
    }

    private async void SelectImageClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not EditDepartmentsPageViewModel vm || vm.SelectedDepartment == null) return;
        var top = TopLevel.GetTopLevel(this);
        if (top?.StorageProvider == null) return;

        var file = (await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Выберите изображение направления",
            FileTypeFilter = [new FilePickerFileType("Изображения") { Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp", "*.webp"] }]
        })).FirstOrDefault();

        if (file == null) return;
        await using var stream = await file.OpenReadAsync();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        vm.SetImage(ms.ToArray(), file.Name);
    }
}
