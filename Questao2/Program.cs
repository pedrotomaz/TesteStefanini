using System.Text.Json;


public class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    private static int getTotalScoredGoals(string team, int year)
    {
        Task<int> team1GoalsTask = GetGoalsAsync(year, team, "team1");
        Task<int> team2GoalsTask = GetGoalsAsync(year, team, "team2");

        int[] results = Task.WhenAll(team1GoalsTask, team2GoalsTask).Result;
        return results[0] + results[1];
    }

    private static async Task<int> GetGoalsAsync(int year, string team, string teamParam)
    {
        int totalGoals = 0;
        int page = 1;
        int totalPages;

        do
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParam}={team}&page={page}";
            string jsonResponse = await _httpClient.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            totalPages = root.GetProperty("total_pages").GetInt32();
            JsonElement data = root.GetProperty("data");

            foreach (JsonElement item in data.EnumerateArray())
            {
                totalGoals += int.Parse(item.GetProperty($"{teamParam}goals").ToString());
            }

            page++;
        } while (page <= totalPages);

        return totalGoals;
    }

}