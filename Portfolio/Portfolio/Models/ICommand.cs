using System.Threading.Tasks;

namespace Portfolio.Models
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        string Usage { get; }
        Task<string> ExecuteAsync(string[] args);
    }
}
