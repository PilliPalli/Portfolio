using Microsoft.AspNetCore.Components.Web;

namespace Portfolio.Pages
{
    public partial class Home
    {
        private List<string> output = new() { "Welcome to your interactive CV. Type 'ls' for commands." };
        private string input = "";

        private Dictionary<string, string> commands = new()
        {
            { "ls", "projects   skills   about_me   contact_me" },
            { "cat about_me", "Name: Dein Name\nBeruf: Softwareentwickler\nSkills: C#, Blazor, Dapper, SQL\nErfahrung: 3 Jahre Softwareentwicklung" },
            { "cat skills", "- C# (5/5)\n- Blazor (4/5)\n- SQL & Dapper (4/5)\n- ASP.NET Core (4/5)\n- Git & CI/CD (4/5)" },
            { "cat projects", "- Schwimmbad-Mitgliedschaftsverwaltung (C#, SQLite, WPF)\n- Garbage-Collection-Tool mit WPF\n- Kundenverwaltung für Oldtimer-Vermietung (Blazor Server)" },
            { "cat contact_me", "Email: deine-email@example.com\nLinkedIn: linkedin.com/in/deinname\nGitHub: github.com/deinname" },
            { "sudo hire_me", "ACCESS GRANTED. Welcome to the team! 🎉" },
            { "clear", "" }
        };

        private void HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            if (input.Trim() == "clear")
            {
                output.Clear();
                output.Add("Welcome to your interactive CV. Type 'ls' for commands.");
            }
            else
            {
                output.Add($"> {input}");

                if (commands.TryGetValue(input.Trim(), out string response))
                {
                    output.Add(response);
                }
                else
                {
                    output.Add("Command not found. Try 'ls' to see available commands.");
                }
            }

            input = "";
        }
    }
}
