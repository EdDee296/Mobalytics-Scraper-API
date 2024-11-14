
using ChampionBuildApi.Models;
using System.Threading.Tasks;

namespace ChampionBuildApi.Services
{
    public interface IChampionScraper
    {
        Task<ChampionBuild> GetChampionBuildAsync(string champion, string role);
    }
}
