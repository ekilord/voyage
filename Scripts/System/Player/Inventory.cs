using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RawResource;
using Newtonsoft.Json;

public class Inventory
{
	[JsonProperty]
	private readonly ResourceHolder Gold;
	[JsonProperty]
	private readonly ResourceHolder Wood;
	[JsonProperty]
	private readonly ResourceHolder Stone;
	[JsonProperty]
	private readonly ResourceHolder Iron;
	[JsonProperty]
	private readonly ResourceHolder Aether;

	public Inventory( int gold, int wood, int stone, int iron, int aether )
	{
		Gold = new( RawResources.GOLD, gold );
		Wood = new( RawResources.WOOD, wood );
		Stone = new( RawResources.STONE, stone );
		Iron = new( RawResources.IRON, iron );
		Aether = new( RawResources.AETHER, aether );
	}

	public Inventory()
	{
		Gold = new( RawResources.GOLD, 0 );
		Wood = new( RawResources.WOOD, 0 );
		Stone = new( RawResources.STONE, 0 );
		Iron = new( RawResources.IRON, 0 );
		Aether = new( RawResources.AETHER, 0 );
	}

	public Inventory( bool cheat )
	{
		Gold = new( RawResources.GOLD, 10 );
		Wood = new( RawResources.WOOD, 10 );
		Stone = new( RawResources.STONE, 10 );
		Iron = new( RawResources.IRON, 10 );
		Aether = new( RawResources.AETHER, 10 );
	}

	public int GetGold()
	{
		return Gold.GetAmount();
	}

	public int GetWood()
	{
		return Wood.GetAmount();
	}

	public int GetStone()
	{
		return Stone.GetAmount();
	}

	public int GetIron()
	{
		return Iron.GetAmount();
	}

	public int GetAether()
	{
		return Aether.GetAmount();
	}

	public bool RemoveCost( Cost cost )
	{
		if ( !cost.IsEnough( this ) )
			return false;

		Gold.DecreaseAmount( cost.GetGoldCost() );
		Wood.DecreaseAmount( cost.GetWoodCost() );
		Stone.DecreaseAmount( cost.GetStoneCost() );
		Iron.DecreaseAmount( cost.GetIronCost() );
		Aether.DecreaseAmount( cost.GetAetherCost() );

		return true;
	}

	public void AddContents( Inventory other )
	{
		Gold.IncreaseAmount( other.GetGold() );
		Wood.IncreaseAmount( other.GetWood() );
		Stone.IncreaseAmount( other.GetStone() );
		Iron.IncreaseAmount( other.GetIron() );
		Aether.IncreaseAmount( other.GetAether() );
	}

	public void AddResource( RawResource resource, int amount )
	{
		switch ( resource.GetResourceType() ) {
			case "Gold":
			Gold.IncreaseAmount( amount );
			break;
			case "Wood":
			Wood.IncreaseAmount( amount );
			break;
			case "Stone":
			Stone.IncreaseAmount( amount );
			break;
			case "Iron":
			Iron.IncreaseAmount( amount );
			break;
			case "Aether":
			Aether.IncreaseAmount( amount );
			break;
		}
	}

	public int GetAllResourceAmount()
	{
		int amount = 0;
		amount += Gold.GetAmount();
		amount += Wood.GetAmount();
		amount += Stone.GetAmount();
		amount += Iron.GetAmount();
		amount += Aether.GetAmount();
		return amount;
	}
}
