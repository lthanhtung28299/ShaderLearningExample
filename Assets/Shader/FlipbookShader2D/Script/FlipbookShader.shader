Shader "CustomSprites/FlipbookShader"
{
    Properties
    {
        [Header(TextureAtlas)]
        _MainTex ("Texture", 2D) = "black" {}
        _TextureWidth("Width", Float) = 0
        _TextureHeight("Height", Float) = 0
        _Tile("Tile", float) = 0

        [Toggle(AUTOLOOP)] _AutoLoop("Auto Loop",Float) = 0
        _Speed("Speed", Range(0,100)) = 0
        [Toggle(FLIPX)] _FlipX("FlipX", Float) = 0
        [Toggle(FLIPY)] _FlipY("FlipY", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "IgnoreProjector" = "True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature AUTOLOOP
            #pragma shader_feature FLIPX
            #pragma shader_feature FLIPY
            #pragma target 2.0
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv_MainTex : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float2 _Flipbook_Invert = float2(FLIPX,FLIPY);

            float2 Unity_Flipbook_float(float2 UV, float Width, float Height, float Tile, float2 Invert, out float2 Out)
            {
                Tile = fmod(Tile, Width * Height);
                float2 tileCount = float2(1.0, 1.0) / float2(Width, Height);
                float tileY = abs(Invert.y * Height - (floor(Tile * tileCount.x) + Invert.y * 1));
                float tileX = abs(Invert.x * Width - ((Tile - Width * floor(Tile * tileCount.x)) + Invert.x * 1));
                return Out = (UV + float2(tileX, tileY)) * tileCount;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TextureWidth;
            float _TextureHeight;
            float _Tile;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float tile = 0;
#if AUTOLOOP
                tile = floor(fmod((_Time.y) * _Speed,_TextureHeight*_TextureWidth));
#else
                tile = floor(fmod(_Tile,_TextureHeight*_TextureWidth));
#endif
                float2 UV = Unity_Flipbook_float(v.uv,_TextureWidth,_TextureHeight,tile,_Flipbook_Invert,v.uv);
                o.uv_MainTex = TRANSFORM_TEX(UV, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float textureAtlasCount = _TextureWidth * _TextureHeight;
                float2 mainTexUV = i.uv_MainTex;
                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                return col;
            }
            ENDCG
        }
    }
}
