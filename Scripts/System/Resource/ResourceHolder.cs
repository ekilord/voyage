using Newtonsoft.Json;
using System;
using UnityEngine;

public class ResourceHolder
{
    [JsonProperty]
    private RawResource Resource;
    [JsonProperty]
    private int Amount;

    public ResourceHolder()
    {
        Resource = RawResource.RawResources.GOLD;
        Amount = 0;
    }

    public ResourceHolder(RawResource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }

	public RawResource GetResourceType()
    {
        return Resource;
    }

    public int GetAmount() { return Amount; }

    public void IncreaseAmount(int amount)
    {
        if (amount < 0) throw new ArgumentException("Argument cannot be negative");

        Amount += amount;
    }

    public void DecreaseAmount(int amount)
    {
        if (amount < 0) throw new ArgumentException("Argument cannot be negative");
        if (Amount - amount < 0) throw new ArgumentException("Amount cannot be negative");

        Amount -= amount;
    }
}
