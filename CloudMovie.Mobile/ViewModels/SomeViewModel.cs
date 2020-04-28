using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CloudMovie.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Essentials;

namespace CloudMovie.Mobile
{
    public class SomeViewModel : CoreViewModel
    {
        public string SomeText { get; set; }
        public int TotalItems { get; set; }
        public ICommand SomeAction { get; set; }
        public ICommand SeeFontAction { get; set; }
        public ICommand GoogleLoginAction { get; set; }
        public ICommand MicrosoftLoginAction { get; set; }

        public string AccountToken { get; set; }

        public SomeViewModel()
        {
            SomeAction = new CoreCommand(async (obj) =>
            {
                LoadingMessageHUD = "Some action...";
                IsLoadingHUD = true;
                await Task.Delay(new TimeSpan(0, 0, 4));
                IsLoadingHUD = false;
            });
            SeeFontAction = new Command(async() => {
                await Navigation.PushAsync(new FontDemo());
            });
            GoogleLoginAction = new CoreCommand(async (obj) =>
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri("https://cloudmovieweb.azurewebsites.net/mobileauth/Google"),
                    new Uri("cloudmovie.mobile://"));

                AccountToken = authResult?.AccessToken;
            });
            MicrosoftLoginAction = new CoreCommand(async (obj) =>
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri("https://cloudmovieweb.azurewebsites.net/mobileauth/Microsoft"),
                    new Uri("cloudmovie.mobile://"));

                AccountToken = authResult?.AccessToken;
            });
        }

        public override void OnViewMessageReceived(string key, object obj)
        {
            //Inter-app communication like MessageCenter without Pub/Sub
        }

        public override void OnInit()
        {
            var items = this.SomeLogic.GetSomeData();
            if (items.error == null)
            {
                TotalItems = items.data.Count;
            }
            else
            {
                this.DialogPrompt.ShowMessage(new Prompt()
                {
                    Title = "Error",
                    Message = items.error.Message
                });
            }
            
        }

        public override void OnRelease(bool includeEvents)
        {
            //Used to release resources - NOT A IMPLEMENTATION OF IDISPOSE
            //Include events mean to unhook all events as well otherwise leave them connected.
        }
    }
}
