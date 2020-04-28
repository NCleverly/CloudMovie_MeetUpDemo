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
                    new Label()
                    {
                        Text="Enter Text:",
                        Margin = new Thickness(5, 5, 5, 0)
                    },
                    new CoreMaskedEntry()
                    {
                        Margin = 5,
                        BackgroundColor = Color.Transparent
                    }.Bind(CoreMaskedEntry.TextProperty, nameof(SomeViewModel.SomeText)),
                    new Label()
                    {
                        Margin = 5
                    }.Bind(Label.TextProperty, nameof(SomeViewModel.SomeText), converter: CoreSettings.UpperText),
                    new CoreButton()
                    {
                        Text="Some Action",
                        Style = CoreStyles.LightOrange,
                        Margin=5,
                    }.Bind(Button.CommandProperty,nameof(SomeViewModel.SomeAction)),
                    new Label()
                    {
                        Margin = 5
                    }.Bind(Label.TextProperty, nameof(SomeViewModel.TotalItems), stringFormat: "Total count is {0}"),
                    new CoreButton()
                    {
                        Text="See Fonts",
                        Style = CoreStyles.LightOrange,
                        Margin=5,
                    }.Bind(Button.CommandProperty,nameof(SomeViewModel.SeeFontAction)),
                    new Grid{
                        ColumnDefinitions = new ColumnDefinitionCollection{
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                        },
                        RowDefinitions = new RowDefinitionCollection{
                            new RowDefinition{Height=new GridLength(1, GridUnitType.Star)}
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