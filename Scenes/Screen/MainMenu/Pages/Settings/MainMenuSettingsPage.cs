using Godot;
using GodotTemplate.Scripts.Service.Settings;
using KludgeBox.DI.Requests.ChildInjection;
using KludgeBox.Godot.Services;

namespace GodotTemplate.Scenes.Screen.MainMenu.Pages.Settings;

public partial class MainMenuSettingsPage : MainMenuPage
{
    
    [Child] public TextEdit NickTextEdit { get; private set; }
    [Child] public TextEdit ColorTextEdit { get; private set; }
    [Child] public OptionButton LanguageOptionButton { get; private set; }
    [Child] public Button SaveReturnButton { get; private set; }
    
    public override void _Ready()
    {
        Di.Process(this);

        GameSettings gameSettings = Services.GameSettings.GetSettings();
        NickTextEdit.Text = gameSettings.PlayerNick;
        ColorTextEdit.Text = gameSettings.PlayerColor.ToHtml(false);
        LanguageOptionButton.Text = Services.I18N.GetLocaleInfoByCode(gameSettings.Locale).Name;
        
        foreach (I18NService.LocaleInfo localeInfo in Services.I18N.Locales)
        {
            LanguageOptionButton.GetPopup().AddItem(localeInfo.Name);
        }
        
        LanguageOptionButton.ItemSelected += _ => Services.I18N.SetCurrentLocale(GetLocaleCodeFromOptionButton());
        SaveReturnButton.Pressed += ParseAndSaveSettings;
    }

    private void ParseAndSaveSettings()
    {
        string nick = NickTextEdit.Text;
        Color color = Color.FromHtml(ColorTextEdit.Text);
        string locale = GetLocaleCodeFromOptionButton();
        
        Services.GameSettings.SetSettings(Services.GameSettings.GetSettings() with
        {
            PlayerNick = nick,
            PlayerColor = color,
            Locale = locale
        });
        Services.I18N.SetCurrentLocale(locale);
        ChangeMenuPage(PackedScenes.Main);
    }

    private string GetLocaleCodeFromOptionButton()
    {
        return Services.I18N.GetLocaleInfoByName(LanguageOptionButton.Text).Code;
    }
}