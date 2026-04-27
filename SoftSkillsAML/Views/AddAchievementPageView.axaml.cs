using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SoftSkillsAML.ViewModels;
using System.IO;
using System.Linq;

namespace SoftSkillsAML;

public partial class AddAchievementPageView : UserControl
{
    public AddAchievementPageView()
    {
        InitializeComponent();
        DataContext = new AddAchievementPageViewModel();
    }

    private async void SelectImageClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not AddAchievementPageViewModel vm)
            return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите изображение достижения",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Изображения")
                {
                    Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp", "*.webp"]
                }
            ]
        });

        var file = files.FirstOrDefault();
        if (file == null)
            return;

        await using var stream = await file.OpenReadAsync();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        vm.SetImage(ms.ToArray(), file.Name);
    }
}
