﻿using LocalyticsMaui.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MauiSDKTester;

public partial class App : Application
{
    public static ILocalytics localytics;
    public static IPlatform platform;

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        localytics = new LocalyticsMauiShared();
        platform = new LocalyticsMauiShared();
    }

    void HandleInboxCampaignsDelegate(IInboxCampaign[] campaigns)
    {
        foreach (IInboxCampaign a in campaigns)
        {
            try
            {
                Debug.WriteLine("HandleInboxCampaignsDelegate inbox campaign " + a);

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }

    protected override void OnStart()
    {
        platform.RegisterEvents();
        Task.Run(() =>
        {
            CommonSmokeTest();
        });
    }

    private void CommonSmokeTest()
    {

        localytics.SetOptions(new Dictionary<string, object>
            {
                {"ll_wifi_upload_interval_seconds", 15},
                {"ll_session_timeout_seconds", 10}
            });

        //localytics.TestModeEnabled = true;
        localytics.OpenSession();
        localytics.CloseSession();
        localytics.TagEvent("Event bEfore opting out");
        localytics.PrivacyOptedOut = true;
        localytics.PrivacyOptedOut = false;

        localytics.OptedOut = true;
        localytics.TagEvent("EventWhenOptedOut");
        localytics.OptedOut = false;

        localytics.TagEvent("TagEvent");
        localytics.Upload();
        localytics.TagEvent("TagEventWithEmptyAttribs", new Dictionary<string, string>());
        localytics.PauseDataUploading(true);
        Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "attr1", "1" }
            };
        localytics.TagEvent("TagEventWithAttribs", dict);
        localytics.Upload();
        localytics.TagEvent("TagEventWithAttribsWithValue", dict, 0);
        localytics.Upload();
        localytics.TagEvent("TagEventWithAttribsWithValue", dict, 10);
        localytics.PauseDataUploading(false);


        localytics.TagPurchased("item", "id", "sample", null, null);
        localytics.TagPurchased("item1", "1", "item", 100, new Dictionary<string, string>());
        localytics.TagAddedToCart("item1", "1", "item", 100, new Dictionary<string, string>());
        localytics.TagStartedCheckout(100, 5, new Dictionary<string, string>());
        localytics.TagCompletedCheckout(100, 5, new Dictionary<string, string>());
        localytics.TagContentViewed("name", "is", "type", new Dictionary<string, string>());
        localytics.TagSearched("query", "type", 5, new Dictionary<string, string>());
        localytics.TagShared("name", "id", "type", "method", new Dictionary<string, string>());
        localytics.TagContentRated("name", "id", "type", 1, new Dictionary<string, string>());
        Customer customer = new Customer("!234", "John", "Appleseed", "John Appleseed", "jappleseed@localytics.com");

        localytics.TagCustomerRegistered(customer, "method", new Dictionary<string, string>());

        localytics.TagCustomerLoggedIn(customer, null, null);
        localytics.TagInvited("invited With no attribs", null);
        localytics.TagInvited("method", new Dictionary<string, string>());
        var dictTagInvited = new Dictionary<string, string>();
        dictTagInvited.Add("key1", "value1");
        localytics.TagInvited("method", dictTagInvited);
        localytics.TagCustomerLoggedOut(new Dictionary<string, string> {
                {"customerId", "1234"}
            });
        localytics.CloseSession();


        localytics.CustomerId = "XamarinFormIOS CustomerId";
        //localytics.TagCustomerLoggedIn(null, "method", new Dictionary<string, string>());
        localytics.SetProfileAttribute("Age", XFLLProfileScope.Organization, "83");
        localytics.SetProfileAttribute("Age", XFLLProfileScope.Application, "3");

        localytics.SetProfileAttribute("Ticker", XFLLProfileScope.Application, "CHAR", "LCTS");



        localytics.AddProfileAttribute("Lucky numbers Set Int", XFLLProfileScope.Application, new int[] { 2221, 3331 });
        localytics.AddProfileAttribute("Lucky numbers Set Long", XFLLProfileScope.Application, new long[] { 2222, 3332 });
        localytics.AddProfileAttribute("Lucky String Set", XFLLProfileScope.Application, new string[] { "2342", "3452" });
        localytics.AddProfileAttribute("Lucky String", XFLLProfileScope.Application, "234", "345");
        localytics.AddProfileAttribute("Lucky numbers", XFLLProfileScope.Application, 222, 333);
        localytics.AddProfileAttribute("Lucky Strings Mixed", XFLLProfileScope.Application, "222", "333", "abc");
        localytics.RemoveProfileAttribute("Lucky numbers", XFLLProfileScope.Application, 222);
        localytics.SetProfileAttribute("Age", XFLLProfileScope.Application, 32);
        localytics.IncrementProfileAttribute(1, "Age");
        localytics.IncrementProfileAttribute(1, "Age");
        localytics.SetProfileAttribute("Age", XFLLProfileScope.Organization, 32);
        localytics.DecrementProfileAttribute(2, "Age", XFLLProfileScope.Organization);

        // Need Data based Profile tests


        localytics.DeleteProfileAttribute("TestDeleteProfileAttribute", XFLLProfileScope.Application);

        localytics.SetCustomerEmail("XamarinFormIOSEmail@localytics.com");
        localytics.SetCustomerFirstName("XamarinFormIOS FirstName");
        localytics.SetCustomerLastName("XamarinFormIOS LastName");
        localytics.SetCustomerFullName("XamarinFormIOS Full Name");

        for (int i = 0; i < 20; i++)
        {
            localytics.SetCustomDimension("XamarinFormIOSCD" + i, (uint)i);
            Task.Run(() =>
            {
                try
                {
                    string dimensionVal = localytics.GetCustomDimension((uint)i);
                    Debug.WriteLine("Dimension " + i + ":" + dimensionVal == null ? "(null)" : dimensionVal);

                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine("Failed to get Dimension " + i + ":" + ex.Message + "\n" + ex.StackTrace);

                }
            });
        }

        localytics.SetIdentifier("test", "id1");
        Debug.WriteLine("Identifier 1:" + localytics.GetIdentifier("id1"));

        localytics.TagEvent("XamarinFormIOS Start");
        localytics.TagScreen("XamarinFormIOS Landing");
        localytics.TagCustomerLoggedOut(new Dictionary<string, string>());
        localytics.Upload();

        localytics.SetInAppMessageDismissButtonHidden(true);
        localytics.SetInAppMessageDismissButtonHidden(false);

        localytics.TriggerInAppMessage("lang");
        localytics.TriggerInAppMessagesForSessionStart();
        localytics.DismissCurrentInAppMessage();

        localytics.RefreshInboxCampaigns(HandleInboxCampaignsDelegate);
        localytics.RefreshAllInboxCampaigns(HandleInboxCampaignsDelegate);

        IInboxCampaign firstInboxCampaign = null;
        localytics.LoggingEnabled = false;
        Task.Run(() =>
        {
            var campaigns = localytics.DisplayableInboxCampaigns();
            foreach (IInboxCampaign campaign in campaigns)
            {
                if (firstInboxCampaign == null)
                {
                    firstInboxCampaign = campaign;
                }
                Debug.WriteLine("inbox campaign " + campaign);
            }

            localytics.LoggingEnabled = false;
            localytics.InboxListItemTapped(firstInboxCampaign);
            localytics.TagImpression(firstInboxCampaign, "custom");
            localytics.DeleteInboxCampaign(firstInboxCampaign);
        });

        //localytics.TagImpressionForInAppCampaign(null, "custom");
        //         localytics.TagImpressionForPushToInboxCampaign(null, true);

        //localytics.TagPlacesPushReceived(null);
        //localytics.TagPlacesPushOpened(null);
        //localytics.TagPlacesPushOpened(null, "123");
        //localytics.TriggerPlacesNotificationForCampaign(null);
        localytics.TriggerPlacesNotificationForCampaignId(1, "1");
    }

    protected override void OnSleep()
    {
        // Handle when your app sleeps
    }

    protected override void OnResume()
    {
        // Handle when your app resumes
    }
}



