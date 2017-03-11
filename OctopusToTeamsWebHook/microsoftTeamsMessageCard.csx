#r "Newtonsoft.Json"

using Newtonsoft.Json;

public class MicrosoftTeamsMessagePotentialAction
{
    [JsonProperty("@context")]
    public string context { get; }  = "http://schema.org";

    [JsonProperty("@type")]
    public string type { get; } = "ViewAction";
    
    public string name { get; set; }
    public string[] target { get; set; }
}

public class MicrosoftTeamsMessageFacts
{
    public string name { get; set; }
    public string value { get; set; }
}

public class MicrosoftTeamsMessageSection
{
    //public string title { get; set; }
    public string activityTitle { get; set; }
    //public string activitySubtitle { get; set; }
    public string activityImage { get; set; }
    public string activityText { get; set; }
    public ICollection<MicrosoftTeamsMessageFacts> facts { get; set; }
    //public string text { get; set; }
    //public bool? markdown { get; set; }
}

public class MicrosoftTeamsMessageCard
{
    public string summary { get; set; }
    public string text { get; set; }
    public string title { get; set; }
    public ICollection<MicrosoftTeamsMessageSection> sections { get; set; }
    public ICollection<MicrosoftTeamsMessagePotentialAction> potentialAction { get; set; }
}