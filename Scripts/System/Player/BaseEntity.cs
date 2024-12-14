using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BaseEntity : AbstractEntity
{
    [JsonProperty]
    private bool Old;
    [JsonProperty]
    private Coordinate? Location;

    public BaseEntity() : base()
    {
        Location = new Coordinate(0,0);
    }

    public BaseEntity(bool old, Coordinate? location) : base(old ? "Old Encampment" : "Encampment", PlayerRelation.PLAYER, UIUtils.Colors.DeepGold)
    {
        Old = old;
        Location = location;
    }

    public bool IsOld()
    {
        return Old;
    }

    public Coordinate? GetLocation()
    {
        return Location;
    }

    public void SetLocation(Coordinate location)
    {
        Location = location;
    }

    public bool AreScoutsHome()
    {
        if (Location == null) return false;

        Dictionary<ScoutEntity, int> scouts = PlayerCharacter.GetScouts();

        int totalAmount = 0;

        foreach ((ScoutEntity scout, int amount) in scouts)
        {
            totalAmount += amount;
        }

        int amountHere = 0;

        if (PlayerCharacter.GetMap().GetTiles()[Location.GetX() + 1, Location.GetY()].GetEntityOccupation() is ScoutEntity) amountHere++;
        if (PlayerCharacter.GetMap().GetTiles()[Location.GetX(), Location.GetY() + 1].GetEntityOccupation() is ScoutEntity) amountHere++;
        if (PlayerCharacter.GetMap().GetTiles()[Location.GetX() - 1, Location.GetY()].GetEntityOccupation() is ScoutEntity) amountHere++;
        if (PlayerCharacter.GetMap().GetTiles()[Location.GetX(), Location.GetY() - 1].GetEntityOccupation() is ScoutEntity) amountHere++;

        return totalAmount <= amountHere;
    }
}
