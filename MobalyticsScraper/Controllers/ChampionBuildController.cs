using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChampionBuildApi.Services;

namespace ChampionBuildApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class ChampionBuildController : ControllerBase
    {
        private readonly IChampionScraper _championScraper;

        public ChampionBuildController(IChampionScraper championScraper)
        {
            _championScraper = championScraper;
        }
        /// <summary>
        /// Retrieves champion build data.
        /// </summary>
        /// <param name="champion">The champion's name (e.g., "yasuo").</param>
        /// <param name="role">The role of the champion (e.g., "mid").</param>
        /// <returns>Champion build information including items, spells, and runes.</returns>
        [HttpGet("{champion}/{role}")]
        public async Task<IActionResult> GetChampionBuild(string champion, string role)
        {
            if (string.IsNullOrWhiteSpace(champion))
            {
                return BadRequest("Champion name is required.");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role is required.");
            }   

            var build = await _championScraper.GetChampionBuildAsync(champion.ToLower().Trim(), role.ToLower().Trim());

            if (build == null)
            {
                return NotFound($"No build data found for champion: {champion} for {role}");
            }

            return Ok(build);
        }
    }
}
