using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace CloudMovie.Mobile
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Some Page";
            //this.Visual = VisualMarker.Material;

            //this.BackgroundImageSource = ImageSource.FromFile("Background.png");

            Content = new StackLayout()
            {
                Padding = 20,
                Children = {
                    new Grid{
                        ColumnDefinitions = new ColumnDefinitionCollection{
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                        },
                        RowDefinitions = new RowDefinitionCollection{
                            new RowDefinition{Height=new GridLength(50)}
                        },
                        Children = {
                            new CoreButton()
                            {
                                Text = "Google Login",
                                Style = CoreStyles.GoogleLogin,
                                Margin = 5,
                            }.Bind(Button.CommandProperty, nameof(SomeViewModel.GoogleLoginAction))
                            .Col(0),
                            new CoreButton()
                            {
                                Text = "Microsoft Login",
                                Style = CoreStyles.LightOrange,
                                Margin = 5,
                            }.Bind(Button.CommandProperty, nameof(SomeViewModel.MicrosoftLoginAction))
                            .Col(1),
                            new CoreButton()
                            {
                                Text = "Custom Login",
                                Style = CoreStyles.LightOrange,
                                Margin = 5,
                            }.Bind(Button.CommandProperty, nameof(SomeViewModel.MicrosoftLoginAction))
                            .Col(1),
                        }
                    },
                    new ScrollView{  Content = new StackLayout()
                    {
                        Padding = 20,
                        Children = {
                            new Label()
                            {
                                 Text="Access token:",
                                Margin = new Thickness(5, 5, 5, 0)
                            },
                            new Label()
                            { 
                                Margin = new Thickness(5, 5, 5, 0)
                            }.Bind(Label.TextProperty, nameof(SomeViewModel.AccountToken)),
                        }
                    }
                    }
                }
            };
        }
    }
}