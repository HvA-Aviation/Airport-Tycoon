using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BuildableAtlas))]
public class AtlasEditor : Editor
{
     SerializedProperty tilesProp;
     private int indexAmount = 0;
     
    private void OnEnable()
    {
        tilesProp = serializedObject.FindProperty("Tiles");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(tilesProp, new GUIContent("Tiles"), true);

        for (int i = 0; i < tilesProp.arraySize; i++)
        {
            SerializedProperty tileEntry = tilesProp.GetArrayElementAtIndex(i);
            SerializedProperty tileTypeProp = tileEntry.FindPropertyRelative("TileType");
            SerializedProperty tileProp = tileEntry.FindPropertyRelative("TileData");

            TileType tileType = (TileType)tileTypeProp.enumValueIndex;

            //reset values when a new list item is added
            if (indexAmount > 0 && i >= indexAmount)
            {
                tileType = TileType.Normal;
                tileProp.managedReferenceValue = null;
            }

            if (tileType == TileType.Normal)
            {
                if (tileProp.managedReferenceValue == null || !(tileProp.managedReferenceValue is NormalTile))
                {
                    tileProp.managedReferenceValue = new NormalTile();
                }
            }
            else if (tileType == TileType.Utility)
            {
                if (tileProp.managedReferenceValue == null || !(tileProp.managedReferenceValue is UtilityTile))
                {
                    tileProp.managedReferenceValue = new UtilityTile();
                }
            }
        }

        indexAmount = tilesProp.arraySize;

        serializedObject.ApplyModifiedProperties();
    }
}
