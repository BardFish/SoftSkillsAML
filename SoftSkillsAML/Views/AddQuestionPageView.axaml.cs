using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SoftSkillsAML.ViewModels;
using System.IO;
using System.Linq;

namespace SoftSkillsAML;

public partial class AddQuestionPageView : UserControl
{
    public AddQuestionPageView()
    {
        InitializeComponent();
        DataContext = new AddQuestionPageViewModel();
    }

    private async void SelectQuestionImageClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not AddQuestionPageViewModel vm) return;
        var top = TopLevel.GetTopLevel(this);
        if (top?.StorageProvider == null) return;
        var file = (await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { AllowMultiple = false, Title = "Изображение задания", FileTypeFilter = [new FilePickerFileType("Изображения"){Patterns=["*.png","*.jpg","*.jpeg","*.bmp","*.webp"]}] })).FirstOrDefault();
        if (file == null) return;
        await using var s = await file.OpenReadAsync(); using var ms = new MemoryStream(); await s.CopyToAsync(ms); vm.SetQuestionImage(ms.ToArray(), file.Name);
    }

    private async void SelectAnswerImageClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not AddQuestionPageViewModel vm || vm.SelectedAnswer == null) return;
        var top = TopLevel.GetTopLevel(this);
        if (top?.StorageProvider == null) return;
        var file = (await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { AllowMultiple = false, Title = "Изображение варианта ответа", FileTypeFilter = [new FilePickerFileType("Изображения"){Patterns=["*.png","*.jpg","*.jpeg","*.bmp","*.webp"]}] })).FirstOrDefault();
        if (file == null) return;
        await using var s = await file.OpenReadAsync(); using var ms = new MemoryStream(); await s.CopyToAsync(ms); vm.SetAnswerImage(ms.ToArray(), file.Name);
    }
}
