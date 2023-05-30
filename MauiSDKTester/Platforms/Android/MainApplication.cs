using Android.App;
using Android.Runtime;
using LocalyticsMaui.Android;
namespace MauiSDKTester;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    override public void OnCreate()
    {
        base.OnCreate();

#if DEBUG
        var localytics = LocalyticsMauiShared.SharedInstance;
        localytics.LoggingEnabled = true;
#endif

        Localytics.AutoIntegrate(this);

    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

