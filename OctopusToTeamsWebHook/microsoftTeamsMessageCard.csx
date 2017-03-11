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
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string title { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string activityTitle { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string activitySubtitle { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string activityImage { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string activityText { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public ICollection<MicrosoftTeamsMessageFacts> facts { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string text { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public bool? markdown { get; set; }
}

public class MicrosoftTeamsMessageCard
{
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string summary { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string text { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public string title { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public ICollection<MicrosoftTeamsMessageSection> sections { get; set; }

    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public ICollection<MicrosoftTeamsMessagePotentialAction> potentialAction { get; set; }
}