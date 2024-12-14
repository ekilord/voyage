using Newtonsoft.Json;
using System.Collections.Generic;
using static RawResource;

#nullable enable
public class LootTable
{
	[JsonProperty]
	private LootTableEntry MainEntry;
	[JsonProperty]
	private List<LootTableEntry>? SecondaryEntries;

	public LootTable()
	{
		MainEntry = LootTables.FOREST.MainEntry;
		SecondaryEntries = null;
	}

	private LootTable( LootTableEntry mainEntry, List<LootTableEntry>? secondaryEntries )
	{
		MainEntry = mainEntry;
		SecondaryEntries = secondaryEntries;
	}

	public LootTable( ResourceHolder mainEntry, List<ResourceHolder> secondaryEntries)
	{
		MainEntry = new LootTableEntry(mainEntry.GetResourceType(), mainEntry.GetAmount());
		
		if (secondaryEntries != null && secondaryEntries.Count > 0)
		{
            List<LootTableEntry> temp = new List<LootTableEntry>();
			foreach (ResourceHolder entry in secondaryEntries)
			{
				temp.Add(new LootTableEntry(entry.GetResourceType(), entry.GetAmount()));
            }

			SecondaryEntries = temp;
        }
	}

	private LootTable( LootTableEntry mainEntry)
	{
		MainEntry = mainEntry;
		SecondaryEntries = null;
	}

	public void RollResults()
	{
		MainEntry.RollResult();

		List<LootTableEntry>? realSecondaryEntries = new();


        if ( SecondaryEntries != null ) {
			foreach ( var entry in SecondaryEntries ) {
				entry.RollResult();
				if ( entry.GetAmount() > 0 ) realSecondaryEntries.Add( entry );
			}

			SecondaryEntries = realSecondaryEntries;
		}
	}

	public LootTableEntry GetMainEntry()
	{
		return MainEntry;
	}

	public List<LootTableEntry>? GetAllSecondaryEntries()
	{
		return SecondaryEntries;
	}

	public LootTableEntry? GetSecondaryEntry( RawResource resource )
	{
		if ( SecondaryEntries == null )
			return null;

		foreach ( var entry in SecondaryEntries ) {
			if ( entry.GetResource().Equals( resource ) ) {
				return entry;
			}
		}

		return null;
	}

	public static class LootTables
	{
		private static readonly ((int Min, int Max) ExtraSmall, (int Min, int Max) Small, (int Min, int Max) Medium, (int Min, int Max) Large) DEPOSITS =
		(
			(5, 15),   // Extra Small
			(10, 20),  // Small
			(20, 30),  // Medium
			(30, 50)   // Large
		);

		private static readonly ((int Min, int Max) Small, (int Min, int Max) Medium, (int Min, int Max) Large) BONUSES =
		(
			(1, 10),  // Small
			(5, 15),  // Medium
			(10, 20)  // Large
		);

        public static LootTable SPARSE_FOREST = new(
            new LootTableEntry(RawResources.WOOD, DEPOSITS.Small, 1f)
        );

        public static LootTable FOREST = new(
			new LootTableEntry( RawResources.WOOD, DEPOSITS.Medium, 1f )
		);

		public static LootTable DENSE_FOREST = new(
			new LootTableEntry( RawResources.WOOD, DEPOSITS.Large, 1f )
		);

		public static LootTable STONE_DEPOSIT = new(
			new LootTableEntry( RawResources.STONE, DEPOSITS.Small, 1f ),  
			new List<LootTableEntry>() {
				new(RawResources.GOLD, BONUSES.Small, 0.25f) 
			}
		);

		public static LootTable IRON_DEPOSIT = new(
			new LootTableEntry( RawResources.IRON, DEPOSITS.Small, 1f ),   
			new List<LootTableEntry>() {
				new(RawResources.AETHER, BONUSES.Small, 0.25f) 
			}
		);

		public static LootTable GOLD_DEPOSIT = new(
			new LootTableEntry( RawResources.GOLD, DEPOSITS.Small, 1f ),   
			new List<LootTableEntry>() {
				new(RawResources.AETHER, BONUSES.Medium, 0.25f) 
			}
		);

		public static LootTable AETHER_DEPOSIT = new(
			new LootTableEntry( RawResources.AETHER, DEPOSITS.Small, 1f )
		);

		public static LootTable RUINS = new(
			new LootTableEntry( RawResources.STONE, DEPOSITS.ExtraSmall, 1f ),   
			new List<LootTableEntry>() {
				new(RawResources.GOLD, BONUSES.Small, 0.3f) 
			}
		);

		public static LootTable TEMPLE_RUINS = new(
			new LootTableEntry( RawResources.AETHER, DEPOSITS.ExtraSmall, 1f ),   
			new List<LootTableEntry>() {
				new(RawResources.GOLD, BONUSES.Small, 0.3f) 
			}
		);


		public static LootTable Copy(LootTable originalLootTable)
		{
            LootTableEntry mainEntryCopy = new(
                    originalLootTable.GetMainEntry().GetResource(),
                    originalLootTable.GetMainEntry().GetMinAmount(),
                    originalLootTable.GetMainEntry().GetMaxAmount(),
                    originalLootTable.GetMainEntry().GetChance()
			);

            List<LootTableEntry>? secondaryEntriesCopy = null;
            if (originalLootTable.GetAllSecondaryEntries() != null)
			{
				List<LootTableEntry>? secondaries = originalLootTable.GetAllSecondaryEntries();

                if (secondaries != null)
				{
                    secondaryEntriesCopy = new List<LootTableEntry>();

                    foreach (var entry in secondaries)
                    {
                        secondaryEntriesCopy.Add(new LootTableEntry(
                            entry.GetResource(),
                            entry.GetMinAmount(),
                            entry.GetMaxAmount(),
                            entry.GetChance()));
                    }
                }
            }

            return new LootTable(mainEntryCopy, secondaryEntriesCopy);
        }
	}
}
