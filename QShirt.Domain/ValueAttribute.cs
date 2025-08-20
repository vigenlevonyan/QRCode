namespace QShirt.Domain;

public class ValueAttribute : Attribute
{
    public ValueAttribute(string value)
    {
        Value = value;
    }
    public string Value { get; }
}
