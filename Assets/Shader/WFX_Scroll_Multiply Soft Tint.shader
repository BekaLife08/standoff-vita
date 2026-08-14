Shader "WFX/Scroll/Multiply Soft Tint" {
	Properties {
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed ("Scroll Speed", Float) = 2
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend DstColor SrcColor, DstColor SrcColor
			ZClip Off
			ZWrite Off
			Cull Off
			Fog {
				Color (0.5,0.5,0.5,0.5)
			}
			GpuProgramID 62132
			// No subprograms found
		}
	}
}