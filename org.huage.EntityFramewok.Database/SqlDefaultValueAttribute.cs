namespace org.huage.EntityFramewok.Database;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqlDefaultValueAttribute : Attribute
{
    /// <summary>
    /// Specifies this property has a default value upon creation.
    /// </summary>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="useAsLiteral">Set to true if the value is <em>not</em> quoted in the DDL.</param>
    public SqlDefaultValueAttribute(object defaultValue, bool useAsLiteral = false)
    {
        DefaultValue = defaultValue;
        UseAsLiteral = useAsLiteral;
    }

    public object DefaultValue { get; private set; }

    /// <summary>
    /// True if the default value is not quoted in the DDL
    /// </summary>
    public bool UseAsLiteral { get; private set; }
}