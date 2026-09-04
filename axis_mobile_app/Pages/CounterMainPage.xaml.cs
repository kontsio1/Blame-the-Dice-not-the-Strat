namespace axis_mobile_app.Pages;

public partial class CounterMainPage
{
    public CounterMainPage(CounterPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}


