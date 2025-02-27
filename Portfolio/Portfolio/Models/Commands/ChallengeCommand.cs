using System.Threading.Tasks;
using Portfolio.Pages;

namespace Portfolio.Models.Commands
{
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
