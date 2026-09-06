namespace axis_mobile_app.Pages;

public partial class ResultsPage
{
    public ResultsPage(MainPageModel model)
    {
        InitializeComponent();
        PageScrollView.HandlerChanged += OnPageScrollViewHandlerChanged;
        BindingContext = model;
    }

#if IOS || MACCATALYST
    private void OnPageScrollViewHandlerChanged(object? sender, EventArgs e)
    {
        if (PageScrollView.Handler?.PlatformView is UIKit.UIScrollView nativeScrollView)
        {
            nativeScrollView.Bounces = false;
            nativeScrollView.AlwaysBounceVertical = false;
        }
    }
#else
    private void OnPageScrollViewHandlerChanged(object? sender, EventArgs e)
    {
        _ = sender;
        _ = e;
    }
#endif
}
