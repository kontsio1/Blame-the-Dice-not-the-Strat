namespace axis_mobile_app.Pages;

public partial class CounterResultsPage
{
    public CounterResultsPage(CounterPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}