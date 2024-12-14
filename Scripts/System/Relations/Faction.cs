[System.Serializable]
public enum PlayerRelation
{
    PLAYER,
    FRIENDLY,
    NEUTRAL,
    HOSTILE,
    NONE
}

[System.Serializable]
public class Faction
{
    private readonly string Name;
    private readonly string Description;
    private PlayerRelation Relation;

    protected Faction(string name, string description, PlayerRelation relation)
    {
        Name = name;
        Description = description;
        Relation = relation;
    }

    public static class Factions
    {
       
    }
}
