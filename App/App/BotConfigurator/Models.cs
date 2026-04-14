using System;
using System.Collections.Generic;

namespace BotConfigurator
{
    public class BotConfig
    {
        public List<BotSection> Sections { get; set; } = new List<BotSection>();
    }

    public class BotSection
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, string> Titles { get; set; } = new Dictionary<string, string> { { "ru", "" }, { "ua", "" } };
        public Dictionary<string, string> Content { get; set; } = new Dictionary<string, string> { { "ru", "" }, { "ua", "" } };
        public List<BotSection> SubSections { get; set; } = new List<BotSection>();

        public override string ToString()
        {
            string title = Titles.ContainsKey("ru") && !string.IsNullOrWhiteSpace(Titles["ru"]) ? Titles["ru"] : "Новый раздел";
            return title;
        }
    }
}
