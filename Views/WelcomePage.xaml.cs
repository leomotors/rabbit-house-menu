﻿#nullable enable

namespace rabbit_house_menu.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class WelcomePage : Page {
    public WelcomePage() {
        InitializeComponent();

        // https://social.msdn.microsoft.com/Forums/sqlserver/en-US/3645fc1b-f7b3-467d-b2d5-489065e8580c/uwp-how-to-get-windows-width-and-height-to-reorganize-bottom-toolbar-buttons?forum=wpdevelop
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
        var bounds = Window.Current.Bounds;
        var height = bounds.Height;

        PageScrollViewer.Height = Math.Max(50, height - 200);
    }

    private const string NoResultFound = "No results found";

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) {
            return;
        }

        List<string> suitableItems = new();
        var splitText = sender.Text.ToLower().Split(" ");

        foreach (var menu in Data.MenusManager.AllMenus) {
            var found = splitText.All((key) => {
                return menu.name.en.ToLower().Contains(key);
            });
            if (found) {
                suitableItems.Add(menu.name.en);
            }
        }

        if (suitableItems.Count == 0) {
            suitableItems.Add(NoResultFound);
        }
        sender.ItemsSource = suitableItems;
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
        var selected = args.SelectedItem.ToString();

        if (selected == NoResultFound) {
            sender.Text = "";
            MenuSearchResult.Text = NoResultFound;
            return;
        }

        foreach (var menu in Data.MenusManager.AllMenus) {
            if (menu.name.en == selected) {
                if (menu.category == "Cursed") {
                    sender.Text = MenuSearchResult.Text = "";
                    RickRoll();
                    return;
                }

                MenuSearchResult.Text = $"{menu.name.ja} ({menu.name.en}) is in \"{menu.category}\" of \"{menu.Restaurant}\"";
                return;
            }
        }

        MenuSearchResult.Text = "Error, that menu magically disappeared";
    }

    private async void RickRoll() {
        // lmao
        await Windows.System.Launcher.LaunchUriAsync(
            new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ")
        );
    }
}
