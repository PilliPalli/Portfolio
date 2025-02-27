using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Portfolio.Services
{
    public enum ThemeType
    {
        Dark,
        Light,
        Hacker,
        Retro,
        Custom
    }

    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private ThemeType _currentTheme = ThemeType.Dark;

        public event Action<ThemeType> OnThemeChanged;

        public ThemeType CurrentTheme => _currentTheme;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            var storedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "terminal_theme");
            if (!string.IsNullOrEmpty(storedTheme) && Enum.TryParse<ThemeType>(storedTheme, out var theme))
            {
                _currentTheme = theme;
                await ApplyThemeAsync(_currentTheme);
            }
            else
            {
                await ApplyThemeAsync(ThemeType.Dark);
            }
        }

        public async Task SetThemeAsync(ThemeType theme)
        {
            if (_currentTheme != theme)
            {
                _currentTheme = theme;
                await ApplyThemeAsync(theme);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "terminal_theme", theme.ToString());
                OnThemeChanged?.Invoke(theme);
            }
        }

        private async Task ApplyThemeAsync(ThemeType theme)
        {
            var rootElement = await _jsRuntime.InvokeAsync<object>("document.documentElement");
            
            switch (theme)
            {
                case ThemeType.Dark:
                    await _jsRuntime.InvokeVoidAsync("eval", @"
                        document.documentElement.style.setProperty('--terminal-bg', '#121212');
                        document.documentElement.style.setProperty('--terminal-text', '#33ff33');
                        document.documentElement.style.setProperty('--terminal-border', '#33ff33');
                        document.documentElement.style.setProperty('--terminal-header-bg', '#121212');
                        document.documentElement.style.setProperty('--terminal-header-text', '#33ff33');
                        document.documentElement.style.setProperty('--body-bg', '#121212');
                    ");
                    break;
                case ThemeType.Light:
                    await _jsRuntime.InvokeVoidAsync("eval", @"
                        document.documentElement.style.setProperty('--terminal-bg', '#f0f0f0');
                        document.documentElement.style.setProperty('--terminal-text', '#121212');
                        document.documentElement.style.setProperty('--terminal-border', '#121212');
                        document.documentElement.style.setProperty('--terminal-header-bg', '#e0e0e0');
                        document.documentElement.style.setProperty('--terminal-header-text', '#121212');
                        document.documentElement.style.setProperty('--body-bg', '#f0f0f0');
                    ");
                    break;
                case ThemeType.Hacker:
                    await _jsRuntime.InvokeVoidAsync("eval", @"
                        document.documentElement.style.setProperty('--terminal-bg', '#000000');
                        document.documentElement.style.setProperty('--terminal-text', '#00ff00');
                        document.documentElement.style.setProperty('--terminal-border', '#00ff00');
                        document.documentElement.style.setProperty('--terminal-header-bg', '#000000');
                        document.documentElement.style.setProperty('--terminal-header-text', '#00ff00');
                        document.documentElement.style.setProperty('--body-bg', '#000000');
                    ");
                    break;
                case ThemeType.Retro:
                    await _jsRuntime.InvokeVoidAsync("eval", @"
                        document.documentElement.style.setProperty('--terminal-bg', '#2b2b2b');
                        document.documentElement.style.setProperty('--terminal-text', '#ff8c00');
                        document.documentElement.style.setProperty('--terminal-border', '#ff8c00');
                        document.documentElement.style.setProperty('--terminal-header-bg', '#2b2b2b');
                        document.documentElement.style.setProperty('--terminal-header-text', '#ff8c00');
                        document.documentElement.style.setProperty('--body-bg', '#2b2b2b');
                    ");
                    break;
                case ThemeType.Custom:
                    // Custom theme values would be loaded from localStorage
                    var customBg = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "terminal_custom_bg") ?? "#121212";
                    var customText = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "terminal_custom_text") ?? "#33ff33";
                    var customBorder = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "terminal_custom_border") ?? "#33ff33";
                    
                    await _jsRuntime.InvokeVoidAsync("eval", $@"
                        document.documentElement.style.setProperty('--terminal-bg', '{customBg}');
                        document.documentElement.style.setProperty('--terminal-text', '{customText}');
                        document.documentElement.style.setProperty('--terminal-border', '{customBorder}');
                        document.documentElement.style.setProperty('--terminal-header-bg', '{customBg}');
                        document.documentElement.style.setProperty('--terminal-header-text', '{customText}');
                        document.documentElement.style.setProperty('--body-bg', '{customBg}');
                    ");
                    break;
            }
        }
    }
}
