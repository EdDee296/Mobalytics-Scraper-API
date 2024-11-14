using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChampionBuildApi.Models;
using System.Web;

namespace ChampionBuildApi.Services
{
    public class ChampionScraper : IChampionScraper
    {
        public async Task<ChampionBuild> GetChampionBuildAsync(string champion)
        {
            var url = $"https://mobalytics.gg/lol/champions/{champion}/build";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var imgElements1 = htmlDocument.DocumentNode.SelectNodes("//img[@class='m-5o4ika']");
            var main = htmlDocument.DocumentNode.SelectSingleNode("//img[@class='m-1iebrlh']");
            var second = htmlDocument.DocumentNode.SelectNodes("//img[@class='m-1nx2cdb']");
            var branch = htmlDocument.DocumentNode.SelectNodes("//div[@class='m-1jwy3wt']");
            var spells = htmlDocument.DocumentNode.SelectNodes("//img[@class='m-d3vnz1']");

            var uniqueAlts = new HashSet<string>();
            var uniqueSpells = new HashSet<string>();
            var items = new List<string>();
            var branchName = new List<string>();
            var runes = new List<string>();
            var spellsList = new List<string>();

            // Process items
            if (imgElements1 != null)
            {
                foreach (var imgElement in imgElements1)
                {
                    var altText = imgElement.GetAttributeValue("alt", "No alt attribute found");
                    string decodedAltText = HttpUtility.HtmlDecode(altText);

                    if (uniqueAlts.Add(decodedAltText))
                    {
                        items.Add(decodedAltText);
                    }
                }
            }

            // Process spells
            if (spells != null)
            {
                foreach (var imgElement in spells)
                {
                    var altText = imgElement.GetAttributeValue("alt", "No alt attribute found");
                    string decodedAltText1 = HttpUtility.HtmlDecode(altText);

                    // Remove "Summoner" prefix if present
                    string cleanedSpell = decodedAltText1.Replace("Summoner", "");

                    // Check if the spell is "Dot" and change it to "Ignite"
                    if (cleanedSpell.Equals("Dot", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanedSpell = "Ignite";
                    }

                    // Add to spellsList only if it's unique
                    if (uniqueSpells.Add(cleanedSpell))
                    {
                        spellsList.Add(cleanedSpell);
                    }
                }
            }


            // Process branch text
            if (branch != null)
            {
                foreach (var divElement in branch)
                {
                    var branchText = divElement.InnerText;
                    string decodedBranchText = HttpUtility.HtmlDecode(branchText);
                    branchName.Add(decodedBranchText);
                }
            }

            // Process main branch
            if (main != null)
            {
                var mainRune = main.GetAttributeValue("alt", "No alt attribute found");
                string decodedMainRune = HttpUtility.HtmlDecode(mainRune);
                runes.Add(decodedMainRune);
            }

            // Process second branch
            if (second != null)
            {
                foreach (var imgElement in second)
                {
                    var altText = imgElement.GetAttributeValue("alt", "No alt attribute found");
                    string decodedAltText = HttpUtility.HtmlDecode(altText);

                    if (uniqueAlts.Add(decodedAltText))
                    {
                        runes.Add(decodedAltText);
                    }
                }
            }

            // Slice items into categories
            var build = new ChampionBuild();
            build.StartingItems = items.Count >= 3 ? items.GetRange(0, 3) : new List<string>();
            build.EarlyItems = items.Count >= 5 ? items.GetRange(3, 2) : new List<string>();
            build.FullBuildItems = items.Count > 5 ? items.GetRange(5, items.Count - 5) : new List<string>();

            // Spells
            build.Spells = spellsList;

            // Slice runes into main and secondary branches
            build.BranchNames = branchName;
            build.MainRunes = runes.Count >= 4 ? runes.GetRange(0, 4) : new List<string>();
            build.SecondaryRunes = runes.Count > 4 ? runes.GetRange(4, runes.Count - 4) : new List<string>();

            return build;
        }
    }
}