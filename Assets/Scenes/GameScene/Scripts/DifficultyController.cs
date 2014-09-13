//using System;
//using UnityEngine;
//
///******************************************************************************/
//[System.Serializable]
//public class DifficultyValueEntry
//{
//    public Color AmbienceColor;
//    public float ScoreThreshold; // This score on up
//        
//    public float NewEnemyFrequencyMin;
//    public float NewEnemyFrequencyMax;
//}
//
///******************************************************************************/
//public class DifficultyController
//{
//    public static DifficultyController instance = null;
//
//    public DifficultyValueEntry[] DifficultyValues = null;
//
//    private TextAsset _difficulty_text = null;
//
//    /// <summary>
//    /// Load creates the instance and loads the data values on instantiation
//    /// </summary>
//    public static void Load()
//    {
//        if ( DifficultyController.instance == null ) {
//            DifficultyController.instance = new DifficultyController();
//        }
//
//        DifficultyController.instance.ReadDifficultyValues();
//    }
//
//    /// <summary>
//    /// Reads the difficulty values from a difficulty.json.txt file in /Resources
//    /// </summary>
//    public void ReadDifficultyValues()
//    {
//        // Have we already done this?
//        if (_difficulty_text != null) return;
//
//        _difficulty_text = UnityEngine.Resources.Load( "difficulty.json" ) as TextAsset;
//
//        JSONObject json = new JSONObject(_difficulty_text.text);
//        if (json == null) {
//            throw new UnityException("Error decoding json");
//        }
//
//        DifficultyController.instance.DifficultyValues = new DifficultyValueEntry[json.Count];
//
//        if (json.type != JSONObject.Type.ARRAY) 
//            throw new UnityException("Error decoding json: not array");
//
//        foreach(JSONObject dif_entry in json.list) {
//            if (dif_entry.type != JSONObject.Type.OBJECT) 
//                throw new UnityException("Error decoding json entry: not object");
//
//            DifficultyValueEntry dve = new DifficultyValueEntry();
//
//            //dve.ScoreThreshold = dif_entry["threshold"];
////            dve.AmbienceColor.r = dif_entry["color"].list[0];
////            dve.AmbienceColor.g = dif_entry["color"][1];
////            dve.AmbienceColor.b = dif_entry["color"][2];
////            Debug.Log(dve.AmbienceColor);
//        }
//
//
//
//    }
//}
//
