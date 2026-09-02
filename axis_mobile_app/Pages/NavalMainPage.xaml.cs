namespace axis_mobile_app.Pages;

public partial class NavalMainPage
{
    public NavalMainPage(NavalPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}

