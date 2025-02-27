using System.Threading.Tasks;
using Portfolio.Services;

namespace Portfolio.Models.Commands
{
    public class ThemeCommand : BaseCommand
    {
        private readonly ThemeService _themeService;

        public override string Name => "theme";
        public override string Description => "Change the terminal theme";
        public override string Usage => "theme [dark|light|hacker|retro|custom]";

        public ThemeCommand(ThemeService themeService)
        {
            _themeService = themeService;
        }

        public override async Task<string> ExecuteAsync(string[] args)
        {
            if (args.Length == 0)
            {
                return $"Current theme: {_themeService.CurrentTheme}\nAvailable themes: dark, light, hacker, retro, custom";
            }

            var themeName = args[0].ToLower();
            
            switch (themeName)
            {
                case "dark":
                    await _themeService.SetThemeAsync(ThemeType.Dark);
                    return "Theme changed to Dark.";
                
                case "light":
                    await _themeService.SetThemeAsync(ThemeType.Light);
                    return "Theme changed to Light.";
                
                case "hacker":
                    await _themeService.SetThemeAsync(ThemeType.Hacker);
                    return "Theme changed to Hacker.";
                
                case "retro":
                    await _themeService.SetThemeAsync(ThemeType.Retro);
                    return "Theme changed to Retro.";
                
                case "custom":
                    await _themeService.SetThemeAsync(ThemeType.Custom);
                    return "Theme changed to Custom. Use 'theme-customize' to set custom colors.";
                
                default:
                    return $"Unknown theme: {themeName}. Available themes: dark, light, hacker, retro, custom";
            }
        }
    }
}
