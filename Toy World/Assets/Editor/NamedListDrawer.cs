using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NamedListAttribute))]
public class NamedListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.ObjectField(rect, property, new GUIContent(((NamedListAttribute)attribute).names[pos]));
        }
        catch
        {
            EditorGUI.ObjectField(rect, property, label);
        }
    }
}
