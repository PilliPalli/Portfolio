using System.Threading.Tasks;
using Portfolio.Pages;

namespace Portfolio.Models.Commands
{
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
}
