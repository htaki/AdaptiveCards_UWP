using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AdaptiveCards.Rendering.Uwp;
using Windows.Data.Json;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdaptiveCards_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AdaptiveCardRenderer _cardRendrer;
        private AdaptiveCard _mainCard;

        public MainPage()
        {
            this.InitializeComponent();

            // Create a card renderer.
            _cardRendrer = new AdaptiveCardRenderer();

            // Set several attributes on the renderer, including the font family and sizes.
            var hostConfig = new AdaptiveHostConfig
            {
                FontFamily = "Calibri",
                FontSizes =
                {
                    Default = 20,
                    Small = 15,
                    Medium = 25,
                    Large = 30,
                    ExtraLarge = 40,
                }
            };

            // Set the host config properties on the renderer.
            _cardRendrer.HostConfig = hostConfig;

            // Create the main card.
            CreateMainCard();

            // Rendering the card enables the actions.
            RenderCard(_mainCard);

        }

        private void RenderCard(AdaptiveCard inputCard)
        {
            try
            {
                // Convert the card to be rendered to a JSON string.
                JsonObject jobj = JsonObject.Parse(inputCard.ToJson().ToString());

                // Render the card from the JSON object.
                RenderedAdaptiveCard renderedCard = _cardRendrer.RenderAdaptiveCardFromJson(jobj);

                // Get the FrameworkElement and attach it to our Frame element in the UI.
                CardFrame.Content = renderedCard.FrameworkElement;



                // Debugging check: Report any renderer warnings.
                // This includes things like an unknown element type found in the card
                // or the card exceeded the maximum number of supported actions, and so on.
                IList<IAdaptiveWarning> warnings = renderedCard.Warnings;

                for (int i = 0; i < warnings.Count; i++)
                {
                    ResultTextBox.Text += warnings[i].Message + "\n";
                }

            }
            catch (Exception ex)
            {
                // Display what went wrong.
                ResultTextBox.Text = ex.Message;
            }
        }

        private void CreateMainCard()
        {
            _mainCard = new AdaptiveCard();

            _mainCard.Body.Add(new AdaptiveTextBlock
            {
                Text = "Adaptive Cards inputs and actions",
                Size = TextSize.ExtraLarge,
                IsSubtle = true
            });

            _mainCard.Body.Add(new AdaptiveTextBlock
            {
                Text = "Click on the images to demo the three actions",
                Size = TextSize.Medium,
                IsSubtle = true
            });

        }
    }
}
