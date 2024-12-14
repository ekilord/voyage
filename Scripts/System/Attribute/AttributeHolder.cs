using Newtonsoft.Json;

public class AttributeHolder
{
    [JsonProperty]
    private Attribute Attribute;
    [JsonProperty]
    private float Value;

    public AttributeHolder()
    {
        Attribute = Attribute.Attributes.HEALTH;
        Value = 0f;
    }

    public AttributeHolder(Attribute attribute, float modifier)
    {
        Attribute = attribute;
        Value = modifier;
    }

    public Attribute GetAttribute()
    {
        return Attribute;
    }

    public float GetValue()
    {
        return Value;
    }
}
