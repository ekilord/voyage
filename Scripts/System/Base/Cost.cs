using UnityEngine;
using static RawResource;

[System.Serializable]
public class Cost
{
	[SerializeField]
	private ResourceHolder GoldCost;
	[SerializeField]
	private ResourceHolder WoodCost;
	[SerializeField]
	private ResourceHolder StoneCost;
	[SerializeField]
	private ResourceHolder IronCost;
	[SerializeField]
	private ResourceHolder AetherCost;

	public Cost()
	{
		GoldCost = new( RawResources.GOLD, 0 );
		WoodCost = new( RawResources.WOOD, 0 );
		StoneCost = new( RawResources.STONE, 0 );
		IronCost = new( RawResources.IRON, 0 );
		AetherCost = new( RawResources.AETHER, 0 );
	}

	public Cost(int goldCost, int woodCost, int stoneCost, int ironCost, int aetherCost)
	{
		GoldCost = new( RawResources.GOLD, goldCost );
		WoodCost = new( RawResources.WOOD, woodCost );
		StoneCost = new( RawResources.STONE, stoneCost );
		IronCost = new( RawResources.IRON, ironCost );
		AetherCost = new( RawResources.AETHER, aetherCost );
	}

	public int GetGoldCost()
	{
		return GoldCost.GetAmount();
	}

	public int GetWoodCost()
	{
		return WoodCost.GetAmount();
	}

	public int GetStoneCost()
	{
		return StoneCost.GetAmount();
	}

	public int GetIronCost()
	{
		return IronCost.GetAmount();
	}

	public int GetAetherCost()
	{
		return AetherCost.GetAmount();
	}

    public override string ToString()
    {
        string costString = "";

        if (GetWoodCost() > 0) costString += $"{GetWoodCost()} W, ";
        if (GetStoneCost() > 0) costString += $"{GetStoneCost()} S, ";
        if (GetIronCost() > 0) costString += $"{GetIronCost()} I, ";
        if (GetGoldCost() > 0) costString += $"{GetGoldCost()} G, ";
        if (GetAetherCost() > 0) costString += $"{GetAetherCost()} A, ";

        if (costString.EndsWith(", "))
            costString = costString.Substring(0, costString.Length - 2);

        return costString;
    }

	public bool IsEnough(Inventory inventory)
	{
		if (inventory.GetWood() < GetWoodCost() || inventory.GetStone() < GetStoneCost() || inventory.GetIron() < GetIronCost() || inventory.GetAether() < GetAetherCost() || inventory.GetGold() < GetGoldCost()) return false;
		return true;
	}
}
