using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

public class Attribute
{
	[JsonProperty]
	private string Name;
	private string Description;

    public Attribute()
    {
        Name = "";
        Description = "";
    }

    protected Attribute(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static class Attributes
    {
        public static Attribute HEALTH = new("Health", "");
        public static Attribute ACTIONS = new("Actions", "");

        public static Attribute PHYSICAL_POWER = new("Physical Power", "");
        public static Attribute ARCANE_POWER = new("Arcane Power", "");
        public static Attribute DEFENSE = new("Defense", "");
        public static Attribute INITIATIVE = new("Initiative", "");
        public static Attribute ACCURACY = new("Accuracy", "");
        public static Attribute MORALE = new("Morale", "");
        public static Attribute SANITY = new("Sanity", "");

        public static List<Attribute> GetAllAttributes()
        {
            return new List<Attribute>()
            {
                HEALTH,
                ACTIONS,
                PHYSICAL_POWER,
                ARCANE_POWER,
                DEFENSE,
                INITIATIVE,
                ACCURACY,
                MORALE,
                SANITY
            };
        }

        public static Attribute GetAttributeByName(string name)
        {
            foreach (Attribute attr in GetAllAttributes())
            {
                if (attr.Name == name) return attr;
            }

            return HEALTH;
        }
    }
}
