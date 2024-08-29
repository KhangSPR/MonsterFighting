/*
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(StarsCondition),true)]
public class ConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label,true);
        if (property.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            SerializedObject so = new SerializedObject(property.objectReferenceValue);

            var soType = property.objectReferenceValue.GetType();
            var fields = soType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach ( var field in fields )
            {
                var prop = so.FindProperty(field.Name);
                if ( prop != null )
                {
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), prop, true);

                }
            }

            so.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label, true);

        if (property.objectReferenceValue != null)
        {
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            var soType = property.objectReferenceValue.GetType();
            var fields = soType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                var prop = so.FindProperty(field.Name);
                if (prop != null)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        return height;
    }
}

[CustomEditor(typeof(StarsCondition), true)]
public class ConditionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This editor is for internal use only.", MessageType.Info);
    }
}*/