using CombatEngine.Schemas;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOCharacterData))]
public class SOCharacterDataEditor : Editor
{
    public override void OnInspectorGUI()
    {

        //Draw default
        base.OnInspectorGUI();

        //Get reference to script object
        SOCharacterData script = (SOCharacterData)target;

        //Check for row 1 changes
        if (script.row0 != null && script.row0.Count > script.columnCount)
        {

            // Remove excess elements if the limit is exceeded
            while (script.row0.Count > script.columnCount)
            {
                //Remove elements after max
                script.row0.RemoveAt(script.row0.Count - 1);
            }

            // Mark the script as dirty so Unity knows to save the changesdirty
            EditorUtility.SetDirty(script);

            //Show error dialog
            EditorUtility.DisplayDialog("Error", $"Maximum character swatch allowed in a row is {script.columnCount}", "OK");
        }

        //Check for row 2 changes
        if (script.row1 != null && script.row1.Count > script.columnCount)
        {
            while (script.row1.Count > script.columnCount)
            {
                //Remove elements after max
                script.row1.RemoveAt(script.row1.Count - 1);
            }

            // Mark the script as dirty so Unity knows to save the changesdirty
            EditorUtility.SetDirty(script);

            //Show error dialog
            EditorUtility.DisplayDialog("Error", $"Maximum character swatch allowed in a row is {script.columnCount}", "OK");
        }
    }
}
