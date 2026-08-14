Shader "WFX/Scroll/Additive" {
	Properties {
		_MainTex ("Looped Texture + Alpha Mask", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
		_ScrollSpeed ("Scroll Speed", Float) = 2
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One One, One One
			ColorMask RGB -1
			ZClip Off
			ZWrite Off
			Cull Off
			GpuProgramID 41517
			// No subprograms found
		}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One One, One One
			ColorMask RGB -1
			ZClip Off
			ZWrite Off
			Cull Off
			Fog {
				Mode 0
			}
			GpuProgramID 84240
			// No subprograms found
		}
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One One, One One
			ColorMask RGB -1
			ZClip Off
			ZWrite Off
			Cull Off
			Fog {
				Mode 0
			}
			GpuProgramID 179872
			// No subprograms found
		}
	}
}