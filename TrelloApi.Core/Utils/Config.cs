using System.Text.Json;
namespace TrelloApi.Core.Utils
{
    public class Settings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiToken { get; set; } = string.Empty;
        public string BoardNamePrefix { get; set; } = "new-board-";
        public string DefaultListName { get; set; } = "New Automation List";
    }
    public static class Config
    {
        public static Settings Load()
        {
            var path = Path.Combine(AppContext.BaseDirectory,
            "appsettings.json");
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json) ?? new
            Settings();
        }
    }
}