using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience
{
	[JsonProperty]
	private int PerkPoints;
	[JsonProperty]
	private int TotalExperience;

	[JsonProperty]
	private int CurrentMovements;
	[JsonProperty]
	private int WonFights;

	[JsonProperty]
	private string? CurrentResearch;

	[JsonProperty]
	private HashSet<string> AcquiredPerks;
	[JsonProperty]
	private HashSet<string> AcquiredResearches;

	public Experience()
	{
		PerkPoints = 0;
		TotalExperience = 0;

		CurrentMovements = 0;
		WonFights = 0;

		CurrentResearch = null;

		AcquiredPerks = new HashSet<string>();
		AcquiredResearches = new HashSet<string>();
	}

	public void AddMovement(int amount)
	{
		CurrentMovements += amount;
	}

    public void AddWonFight(int amount)
    {
        WonFights += amount;
    }

    public string GetCurrentResearch()
	{
		return CurrentResearch;
	}

	public int GetPerkPoints()
	{
		return PerkPoints;
	}

	public bool DecreasePerkPoints(int amount)
	{
		if (amount > 0 && GetPerkPoints() >= amount)
		{
			PerkPoints -= amount;
			return true;
		}

		return false;
	}

	public void AddPerk( string perkName )
	{
		if ( Perks.GetAllPerks().Exists( perk => perk.GetName() == perkName ) && !AcquiredPerks.Contains( perkName ) ) {
			AcquiredPerks.Add( perkName );
		}
	}

	public bool HasPerk( string perkName )
	{
		return AcquiredPerks.Contains( perkName );
	}

	public void AddResearch( string researchName )
	{
		if ( Research.GetAllResearch().Contains( researchName ) && !AcquiredResearches.Contains( researchName )) {
			CurrentResearch = researchName;
		}
	}

	public bool HasResearch( string researchName )
	{
		return AcquiredResearches.Contains( researchName );
	}

	public void AcquireResearch()
	{
		if (CurrentResearch != null)
		{
			AcquiredResearches.Add( CurrentResearch );
			CurrentResearch = null;
		}
	}

	public List<string> GetAllAcquiredPerks()
	{
		return new List<string>( AcquiredPerks );
	}

	public List<string> GetAllAcquiredResearches()
	{
		return new List<string>( AcquiredResearches );
	}

	public void EndRun()
	{
		ConvertToExperience();
		ConvertExperienceToPerk();
		AcquireResearch();
	}

	public void ConvertToExperience()
	{
		TotalExperience += CurrentMovements * 50 + WonFights * 400;
		CurrentMovements = 0;
		WonFights = 0;
	}

	public void ConvertExperienceToPerk()
	{
		PerkPoints += TotalExperience / 1000;
		TotalExperience = TotalExperience % 1000;
	}

    public List<string> GetAvailablePerkNames()
    {
        List<string> availablePerkNames = new List<string>();
        var perkGroups = Perks.GetPerkGroups();

        foreach (var group in perkGroups)
        {
            List<Effect> perksInGroup = group.Value;

            for (int i = 0; i < perksInGroup.Count; i++)
            {
                string perkName = perksInGroup[i].GetName();

                if (!AcquiredPerks.Contains(perkName))
                {
                    if (( i > 0 && AcquiredPerks.Contains(perksInGroup[i - 1].GetName())) || i == 0)
                    {
                        availablePerkNames.Add(perkName);
                    }
                }
            }
        }

        return availablePerkNames;
    }

    public List<string> GetAvailableResearchNames()
    {
        List<string> availableResearchNames = new List<string>();

        foreach (string research in Research.GetAllResearch())
        {
            if (!AcquiredResearches.Contains(research))
            {
                availableResearchNames.Add(research);
            }
        }

        return availableResearchNames;
    }

    public static class Perks
	{
		// Attack Perks
		public static Effect STRENGTH_I = new PermanentEffect( "Strength I", "Increase physical power by 10%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.PHYSICAL_POWER, 0.1f ) );
		public static Effect STRENGTH_II = new PermanentEffect( "Strength II", "Increase physical power by 20%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.PHYSICAL_POWER, 0.2f ) );
		public static Effect STRENGTH_III = new PermanentEffect( "Strength III", "Increase physical power by 30%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.PHYSICAL_POWER, 0.3f ) );

		// Arcane Power Perks
		public static Effect SORCERY_I = new PermanentEffect( "Sorcery I", "Increase arcane power by 10%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ARCANE_POWER, 0.1f ) );
		public static Effect SORCERY_II = new PermanentEffect( "Sorcery II", "Increase arcane power by 20%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ARCANE_POWER, 0.2f ) );
		public static Effect SORCERY_III = new PermanentEffect( "Sorcery III", "Increase arcane power by 30%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ARCANE_POWER, 0.3f ) );

		// Defense Perks
		public static Effect BULWARK_I = new PermanentEffect( "Bulwark I", "Increase defense by 10%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.DEFENSE, 0.1f ) );
		public static Effect BULWARK_II = new PermanentEffect( "Bulwark II", "Increase defense by 20%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.DEFENSE, 0.2f ) );
		public static Effect BULWARK_III = new PermanentEffect( "Bulwark III", "Increase defense by 30%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.DEFENSE, 0.3f ) );

		// Health Perks
		public static Effect FORTITUDE_I = new PermanentEffect( "Fortitude I", "Increase health by 10%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.HEALTH, 0.1f ) );
		public static Effect FORTITUDE_II = new PermanentEffect( "Fortitude II", "Increase health by 20%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.HEALTH, 0.2f ) );
		public static Effect FORTITUDE_III = new PermanentEffect( "Fortitude III", "Increase health by 30%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.HEALTH, 0.3f ) );

		// Max Actions Perks
		public static Effect AGILITY_I = new PermanentEffect( "Agility I", "Gain +1 action per turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACTIONS, 1 ) );
		public static Effect AGILITY_II = new PermanentEffect( "Agility II", "Gain +2 actions per turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACTIONS, 2 ) );
		public static Effect AGILITY_III = new PermanentEffect( "Agility III", "Gain +3 actions per turn.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACTIONS, 3 ) );

		// Accuracy Perks
		public static Effect FOCUS_I = new PermanentEffect( "Focus I", "Increase accuracy by 5%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACCURACY, 0.05f ) );
		public static Effect FOCUS_II = new PermanentEffect( "Focus II", "Increase accuracy by 10%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACCURACY, 0.1f ) );
		public static Effect FOCUS_III = new PermanentEffect( "Focus III", "Increase accuracy by 15%.", EffectSource.PLAYER, PlayerRelation.PLAYER, EffectType.PERMANENT, new AttributeHolder( Attribute.Attributes.ACCURACY, 0.15f ) );


		public static Dictionary<string, List<Effect>> GetPerkGroups()
		{
			return new Dictionary<string, List<Effect>>()
			{
				{ "PP", new List<Effect>() { STRENGTH_I, STRENGTH_II, STRENGTH_III } },
				{ "AP", new List<Effect>() { SORCERY_I, SORCERY_II, SORCERY_III } },
				{ "D", new List<Effect>() { BULWARK_I, BULWARK_II, BULWARK_III } },
				{ "HP", new List<Effect>() { FORTITUDE_I, FORTITUDE_II, FORTITUDE_III } },
				{ "ACT", new List<Effect>() { AGILITY_I, AGILITY_II, AGILITY_III } },
				{ "ACC", new List<Effect>() { FOCUS_I, FOCUS_II, FOCUS_III } }
			};
		}


		public static List<Effect> GetAllPerks()
		{
			return new List<Effect>() {
				STRENGTH_I,
				STRENGTH_II,
				STRENGTH_III,
				SORCERY_I,
				SORCERY_II,
				SORCERY_III,
				BULWARK_I,
				BULWARK_II,
				BULWARK_III,
				FORTITUDE_I,
				FORTITUDE_II,
				FORTITUDE_III,
				AGILITY_I,
				AGILITY_II,
				AGILITY_III,
				FOCUS_I,
				FOCUS_II,
				FOCUS_III
			};
		}

		public static Effect GetPerkByName( string name )
		{
			foreach ( Effect effect in GetAllPerks() ) {
				if ( effect.GetName() == name )
					return effect;
			}

			return STRENGTH_I;
		}
	}

	public static class Research
	{
		private static Dictionary<string, System.Action> researchFunctions = new Dictionary<string, System.Action>()
		{
			{ "Scout Movement Upgrade", ApplyScoutMovementUpgrade },
			{ "Advanced Weaponry", ApplyAdvancedWeaponry },
			{ "Enhanced Defenses", ApplyEnhancedDefenses }
		};


		public static List<string> GetAllResearch()
		{
			return new List<string>( researchFunctions.Keys );
		}

		private static void ApplyScoutMovementUpgrade()
		{
			
		}

		private static void ApplyAdvancedWeaponry()
		{
			
		}

		private static void ApplyEnhancedDefenses()
		{
			
		}
	}
}
