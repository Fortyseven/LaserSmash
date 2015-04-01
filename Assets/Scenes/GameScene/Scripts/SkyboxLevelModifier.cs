using System;
using UnityEditor;
using UnityEngine;

public class SkyboxLevelModifier : MonoBehaviour
{
    public void Update()
    {
        int score = GameController.instance.State.Score;

        float hue = ( 1.0f / 10000 ) * score;
        float adj_hue = hue % 1.0f;

        Color col = EditorGUIUtility.HSVToRGB( adj_hue, 0.52f, 0.82f );

        RenderSettings.fogColor = col;
    }
}
