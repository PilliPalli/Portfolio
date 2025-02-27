using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Portfolio.Models;
using Portfolio.Models.Commands;
using Portfolio.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Pages
{
    public partial class Home : IDisposable
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private CommandService CommandService { get; set; }
        [Inject] private NavigationService NavigationService { get; set; }
        [Inject] private ThemeService ThemeService { get; set; }
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

        private List<OutputLine> output = new();
        private string input = "";
        private ElementReference inputElement;
        private bool showContactForm = false;
        private bool showChallenge = false;
        private int points = 0;

        protected override async Task OnInitializedAsync()
        {
            // Register commands
            RegisterCommands();

            // Initialize theme
            await ThemeService.InitializeAsync();
            ThemeService.OnThemeChanged += HandleThemeChanged;

            // Subscribe to navigation events
            NavigationService.OnNavigate += HandleNavigate;

            // Add welcome message
            output.Add(new OutputLine("Welcome to your interactive Terminal Portfolio. Type 'help' for available commands."));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initTerminal");
                await FocusInputAsync();
            }
        }

        private void RegisterCommands()
        {
            // Help command
            CommandService.RegisterCommand(new HelpCommand(CommandService));

            // Navigation commands
            CommandService.RegisterCommand(new CdCommand(NavigationService));
            CommandService.RegisterCommand(new PwdCommand(NavigationService));
            CommandService.RegisterCommand(new LsCommand(NavigationService));
            CommandService.RegisterCommand(new BackCommand(NavigationService));
            CommandService.RegisterCommand(new HomeCommand(NavigationService));

            // System commands
            CommandService.RegisterCommand(new ClearCommand());
            CommandService.RegisterCommand(new EchoCommand());
            CommandService.RegisterCommand(new DateCommand());
            CommandService.RegisterCommand(new WhoamiCommand());

            // Theme commands
            CommandService.RegisterCommand(new ThemeCommand(ThemeService));

            // Easter egg commands
            CommandService.RegisterCommand(new MatrixCommand());
            CommandService.RegisterCommand(new HireMeCommand());
            CommandService.RegisterCommand(new AsciiArtCommand());

            // Resume command
            CommandService.RegisterCommand(new ResumeCommand(JSRuntime));

            // GitHub command
            CommandService.RegisterCommand(new GitHubCommand(HttpClientFactory.CreateClient()));

            // Custom commands
            CommandService.RegisterCommand(new ContactCommand(this));
            CommandService.RegisterCommand(new ChallengeCommand(this));
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "Enter":
                    await ExecuteCommandAsync();
                    break;
                case "ArrowUp":
                    input = CommandService.GetPreviousCommand();
                    StateHasChanged();
                    break;
                case "ArrowDown":
                    input = CommandService.GetNextCommand();
                    StateHasChanged();
                    break;
                case "Tab":
                    // Auto-complete functionality could be added here
                    break;
            }
        }

        private async Task ExecuteCommandAsync()
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            var commandInput = input.Trim();
            output.Add(new OutputLine($"> {commandInput}", isCommand: true));

            var result = await CommandService.ExecuteCommandAsync(commandInput);

            if (result.ShouldClear)
            {
                output.Clear();
                output.Add(new OutputLine("Welcome to your interactive Terminal Portfolio. Type 'help' for available commands."));
            }

            if (!string.IsNullOrEmpty(result.Output))
            {
                if (result.IsHtml)
                {
                    output.Add(new OutputLine(result.Output, isHtml: true));
                }
                else if (result.IsAnimation)
                {
                    // Handle animation
                    await JSRuntime.InvokeVoidAsync("startMatrix");
                }
                else
                {
                    foreach (var line in result.Output.Split('\n'))
                    {
                        output.Add(new OutputLine(line));
                    }
                }
            }

            input = "";
            await FocusInputAsync();
            StateHasChanged();

            // Scroll to bottom
            await JSRuntime.InvokeVoidAsync("eval", "document.getElementById('terminal-body').scrollTop = document.getElementById('terminal-body').scrollHeight");
        }

        private async Task FocusInputAsync()
        {
            await JSRuntime.InvokeVoidAsync("eval", "document.querySelector('.terminal-input input').focus()");
        }

        private void HandleThemeChanged(ThemeType theme)
        {
            StateHasChanged();
        }

        private void HandleNavigate(NavigationSection section)
        {
            StateHasChanged();
        }

        public void ShowContactForm(bool show)
        {
            showContactForm = show;
            showChallenge = false;
            StateHasChanged();
        }

        public void ShowChallenge(bool show)
        {
            showChallenge = show;
            showContactForm = false;
            StateHasChanged();
        }

        public void HandlePointsEarned(int earnedPoints)
        {
            points += earnedPoints;
            output.Add(new OutputLine($"You earned {earnedPoints} points! Total: {points}", isSuccess: true));
            StateHasChanged();
        }

        public void Dispose()
        {
            // Unsubscribe from events
            ThemeService.OnThemeChanged -= HandleThemeChanged;
            NavigationService.OnNavigate -= HandleNavigate;
        }
    }

    public class OutputLine
    {
        public string Content { get; set; }
        public bool IsCommand { get; set; }
        public bool IsError { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsHtml { get; set; }

        public OutputLine(string content, bool isCommand = false, bool isError = false, bool isSuccess = false, bool isHtml = false)
        {
            Content = content;
            IsCommand = isCommand;
            IsError = isError;
            IsSuccess = isSuccess;
            IsHtml = isHtml;
        }
    }

    public class ContactCommand : BaseCommand
    {
        private readonly Home _home;

        public override string Name => "contact";
        public override string Description => "Display the contact form";
        public override string Usage => "contact";

        public ContactCommand(Home home)
        {
            _home = home;
        }

        public override Task<string> ExecuteAsync(string[] args)
        {
            _home.ShowContactForm(true);
            return Task.FromResult("Showing contact form...");
        }
    }

    public class ChallengeCommand : BaseCommand
    {
        private readonly Home _home;

        public override string Name => "challenge";
        public override string Description => "Start a terminal challenge";
        public override string Usage => "challenge";

        public ChallengeCommand(Home home)
        {
            _home = home;
        }

        public override Task<string> ExecuteAsync(string[] args)
        {
            _home.ShowChallenge(true);
            return Task.FromResult("Starting terminal challenge...");
        }
    }
}
