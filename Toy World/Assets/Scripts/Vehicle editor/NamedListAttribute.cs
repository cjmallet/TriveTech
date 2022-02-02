using UnityEngine;

/// <summary>
/// Editor script that shows the parts attached in the logical order.
/// </summary>
public class NamedListAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedListAttribute(string[] names) { this.names = names; }
}
