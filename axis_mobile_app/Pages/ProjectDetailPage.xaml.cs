using axis_mobile_app.Models;

namespace axis_mobile_app.Pages;

public partial class ProjectDetailPage : ContentPage
{
    public ProjectDetailPage(ProjectDetailPageModel model)
    {
        InitializeComponent();

        BindingContext = model;
    }
}