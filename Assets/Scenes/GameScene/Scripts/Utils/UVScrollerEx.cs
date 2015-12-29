/************************************************************************
** UVScrollerEx.cs
**
** Ported from UVScroller.js 
** 
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Game
{
    [ExecuteInEditMode]
    public class UVScrollerEx : MonoBehaviour
    {
        public float ScrollSpeed = 1.0f;
        public float MainOffsetX = 0.0f;
        public float MainOffsetY = 0.0f;

        public Texture UseCustomTex = null;
        public string CustomTexName = "";

        private Renderer _renderer;
        private Vector2 _offs;

        public void Start()
        {
            _renderer = GetComponent<Renderer>();
            _offs = new Vector2( 0, 0 );
        }

        public void Update()
        {
            float offset = Time.time*ScrollSpeed;
            _offs.x = MainOffsetX * offset;
            _offs.y = MainOffsetY * offset;

            _renderer.sharedMaterial.SetTextureOffset( UseCustomTex ? CustomTexName : "_MainTex", _offs );
        }
    }
}