Shader "WFX/Scroll/Smoke" {
	Properties {
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed ("Scroll Speed", Float) = 2
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend DstColor SrcAlpha, DstColor SrcAlpha
			ZClip Off
			ZWrite Off
			Cull Off
			Fog {
				Color (0.5,0.5,0.5,0.5)
			}
			GpuProgramID 47965
			// No subprograms found
		}
	}
}