﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChampionBuildApi.Services;
using ChampionBuildApi.Models;

namespace ChampionBuildApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Champion Build Endpoints")]
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
        /// <returns>Champion build information including items, spells, and runes.</returns>
        [HttpGet("{champion}")]
        [ProducesResponseType(typeof(ChampionBuild), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetChampionBuild(string champion)
        {
            if (string.IsNullOrWhiteSpace(champion))
            {
                return BadRequest("Champion name is required.");
            }

            var build = await _championScraper.GetChampionBuildAsync(champion.ToLower().Trim());

            if (build == null)
            {
                return NotFound($"No build data found for champion: {champion}");
            }

            return Ok(build);
        }
    }
}