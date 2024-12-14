using Newtonsoft.Json;
using static Attribute;

public class Stats
{
    [JsonProperty]
    private AttributeHolder BaseHealth;
    [JsonProperty]
    private AttributeHolder BaseActions;
    [JsonProperty]
    private AttributeHolder BasePhysicalPower;
    [JsonProperty]
    private AttributeHolder BaseArcanePower;
    [JsonProperty]
    private AttributeHolder BaseDefense;
    [JsonProperty]
    private AttributeHolder BaseInitiative;
    [JsonProperty]
    private AttributeHolder BaseAccuracy;
    [JsonProperty]
    private AttributeHolder BaseMorale;
    [JsonProperty]
    private AttributeHolder BaseSanity;

	public Stats()
	{
		BaseHealth = new AttributeHolder( Attributes.HEALTH, 0f );
		BaseActions = new AttributeHolder( Attributes.ACTIONS, 0f );
		BasePhysicalPower = new AttributeHolder( Attributes.PHYSICAL_POWER, 0f );
		BaseArcanePower = new AttributeHolder( Attributes.ARCANE_POWER, 0f );
		BaseDefense = new AttributeHolder( Attributes.DEFENSE, 0f );
		BaseInitiative = new AttributeHolder( Attributes.INITIATIVE, 0f );
		BaseAccuracy = new AttributeHolder( Attributes.ACCURACY, 0f );
		BaseMorale = new AttributeHolder( Attributes.MORALE, 0f );
		BaseSanity = new AttributeHolder( Attributes.SANITY, 0f );
	}

	public Stats( float healthValue, float actionsValue, float physicalPowerModifier, float arcanePowerModifier, float defenseModifier, float intitiativeModifier, float accuracyModifier, float moraleModifier, float sanityModifier )
	{
        BaseHealth = new AttributeHolder(Attributes.HEALTH, healthValue);
        BaseActions = new AttributeHolder(Attributes.ACTIONS, actionsValue);
        BasePhysicalPower = new AttributeHolder( Attributes.PHYSICAL_POWER, physicalPowerModifier );
		BaseArcanePower = new AttributeHolder( Attributes.ARCANE_POWER, arcanePowerModifier );
        BaseDefense = new AttributeHolder(Attributes.DEFENSE, defenseModifier);
        BaseInitiative = new AttributeHolder( Attributes.INITIATIVE, intitiativeModifier );
		BaseAccuracy = new AttributeHolder( Attributes.ACCURACY, accuracyModifier );
		BaseMorale = new AttributeHolder( Attributes.MORALE, moraleModifier );
		BaseSanity = new AttributeHolder( Attributes.SANITY, sanityModifier );
	}

	public static Stats CreateDefaultStats()
	{
		return new Stats( 20f, 3f, 1f, 1f, 0.5f, 0.5f, 0.8f, 1f, 1f );
	}

    public AttributeHolder GetBaseHealth()
    {
        return BaseHealth;
    }
    public AttributeHolder GetBaseActions()
    {
        return BaseActions;
    }

    public AttributeHolder GetBasePhysicalPower()
	{
		return BasePhysicalPower;
	}

	public AttributeHolder GetBaseArcanePower()
	{
		return BaseArcanePower;
	}

	public AttributeHolder GetBaseDefense()
	{
		return BaseDefense;
	}

	public AttributeHolder GetBaseInitiative()
	{
		return BaseInitiative;
	}

	public AttributeHolder GetBaseAccuracy()
	{
		return BaseAccuracy;
	}

	public AttributeHolder GetBaseMorale()
	{
		return BaseMorale;
	}

	public AttributeHolder GetBaseSanity()
	{
		return BaseSanity;
	}

    public static Stats CreateGhoulsStats()
	{
		return new Stats( 30f, 3f, 0.6f, 0.8f, 0.2f, 0.6f, 0.9f, 0.8f, 0.5f );
	}

	public static Stats CreateGoblinsStats()
	{
		return new Stats( 40f, 3f, 0.7f, 0.7f, 0.3f, 0.7f, 1.0f, 1.0f, 0.6f );
	}

	public static Stats CreateSkeletonsStats()
	{
		return new Stats( 50f, 3f, 0.8f, 0.9f, 0.4f, 0.7f, 1.1f, 1.2f, 0.7f );
	}

	public static Stats CreateGhostsStats()
	{
		return new Stats( 60f, 3f, 0.9f, 1.5f, 0.5f, 0.8f, 1.1f, 0.7f, 0.3f );
	}

	public static Stats CreateWolfPackStats()
	{
		return new Stats( 70f, 3f, 1.0f, 0.6f, 0.5f, 1.0f, 1.2f, 1.4f, 0.8f );
	}

	public static Stats CreateFerociousBearStats()
	{
		return new Stats( 80f, 3f, 1.2f, 0.5f, 0.6f, 0.9f, 1.0f, 1.5f, 0.9f );
	}
}
