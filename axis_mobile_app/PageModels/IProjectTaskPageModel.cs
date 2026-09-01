using CommunityToolkit.Mvvm.Input;
using axis_mobile_app.Models;

namespace axis_mobile_app.PageModels;

public interface IProjectTaskPageModel
{
    IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
    bool IsBusy { get; }
}