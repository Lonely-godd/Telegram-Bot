using System;
using System.Collections.Generic;

namespace BotConfigurator
{
    public class BotConfig
    {
        public List<BotSection> Sections { get; set; } = new();
    }

    public class BotSection
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Dictionary<string, string> Titles { get; set; } = new() { ["ru"] = "", ["ua"] = "" };
        public Dictionary<string, string> Content { get; set; } = new() { ["ru"] = "", ["ua"] = "" };
        public List<BotSection> SubSections { get; set; } = new();

        public override string ToString() =>
            Titles.TryGetValue("ru", out var t) && !string.IsNullOrWhiteSpace(t) ? t : "Новый раздел";
    }
}