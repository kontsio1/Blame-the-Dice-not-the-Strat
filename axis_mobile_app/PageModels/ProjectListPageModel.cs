using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using axis_mobile_app.Data;
using axis_mobile_app.Models;
using axis_mobile_app.Services;

namespace axis_mobile_app.PageModels;

public partial class ProjectListPageModel : ObservableObject
{
    private readonly ProjectRepository _projectRepository;

    [ObservableProperty] private List<Project> _projects = [];

    [ObservableProperty] private Project? selectedProject;

    public ProjectListPageModel(ProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        Projects = await _projectRepository.ListAsync();
    }

    [RelayCommand]
    Task? NavigateToProject(Project project)
        => project is null ? Task.CompletedTask : Shell.Current.GoToAsync($"project?id={project.ID}");

    [RelayCommand]
    async Task AddProject()
    {
        await Shell.Current.GoToAsync($"project");
    }
}