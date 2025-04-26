using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object) , true)]
public class ContextMenuButtonEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI();

        Object targetObject = target;
        MethodInfo [] methods = targetObject.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(m => m.GetCustomAttributes(typeof(ContextMenu) , false).Length > 0)
            .ToArray();

        foreach (MethodInfo method in methods)
        {
            ContextMenu attribute = (ContextMenu)method.GetCustomAttributes(typeof(ContextMenu) , false) [0];
            if (GUILayout.Button(attribute.menuItem))
            {
                method.Invoke(targetObject , null);
            }
        }
    }
}
