using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RawResource
{
    [JsonProperty]
    private string ResourceType;
    private string Description;

    public RawResource()
    {
        ResourceType = "";
        Description = "";
    }

    protected RawResource(string type, string description) 
    {
        ResourceType = type;
        Description = description;
    }

    public string GetResourceType() { return ResourceType; }

    public string GetDescription() { return Description; }

    public static class RawResources
    {
        public static readonly RawResource GOLD = new("Gold", "");
        public static readonly RawResource WOOD = new("Wood", "");
        public static readonly RawResource STONE = new("Stone", "");
        public static readonly RawResource IRON = new("Iron", "");
        public static readonly RawResource AETHER = new("Aether", "");

        public static List<RawResource> GetAllResources()
        {
            return new List<RawResource>()
            {
                GOLD, WOOD, STONE, IRON, AETHER
            };
        }

        public static RawResource GetResourceFromName(string name)
        {
            foreach (RawResource resource in GetAllResources())
            {
                if (resource.GetResourceType().Equals(name)) return resource;
            }

            return GOLD;
        }
    }
}
