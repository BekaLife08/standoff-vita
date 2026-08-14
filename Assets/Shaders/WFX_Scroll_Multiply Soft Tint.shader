// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "WFX/Scroll/Multiply Soft Tint"
{
  Properties
  {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Texture", 2D) = "white" {}
    _ScrollSpeed ("Scroll Speed", float) = 2
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Blend DstColor SrcColor
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4 _Time;
      uniform float4 _TintColor;
      uniform sampler2D _MainTex;
      uniform float _ScrollSpeed;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float2 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = tmpvar_1;
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          out_v.xlv_COLOR = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float2 tmpvar_1;
          tmpvar_1 = in_f.xlv_TEXCOORD0;
          float4 tex_2;
          float mask_3;
          float tmpvar_4;
          tmpvar_4 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0).w * in_f.xlv_COLOR.w);
          mask_3 = tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5 = (_Time * _ScrollSpeed);
          float4 tmpvar_6;
          tmpvar_6 = frac(abs(tmpvar_5));
          float tmpvar_7;
          if((tmpvar_5.x>=0))
          {
              tmpvar_7 = tmpvar_6.x;
          }
          else
          {
              tmpvar_7 = (-tmpvar_6.x);
          }
          tmpvar_1.y = (in_f.xlv_TEXCOORD0.y - tmpvar_7);
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, tmpvar_1);
          tex_2.w = tmpvar_8.w;
          tex_2.xyz = (tmpvar_8.xyz * (in_f.xlv_COLOR.xyz * _TintColor.xyz));
          float4 tmpvar_9;
          tmpvar_9 = lerp(float4(0.5, 0.5, 0.5, 0.5), tex_2, float4(mask_3, mask_3, mask_3, mask_3));
          tex_2 = tmpvar_9;
          out_f.color = tex_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
