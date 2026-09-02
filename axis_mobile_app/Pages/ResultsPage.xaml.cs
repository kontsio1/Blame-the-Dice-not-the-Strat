namespace axis_mobile_app.Pages;

public partial class ResultsPage
{
    public ResultsPage(MainPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}
