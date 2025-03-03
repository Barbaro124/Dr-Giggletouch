// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/DistanceGradient"
{
	Properties
	{
		_ColorFront("Color Front", Color) = (0,1,0.7517242,0)
		_ColorBack("Color Back", Color) = (0,1,0.7517242,0)
		_Offset("Offset", Float) = 0
		_Distance("Distance", Float) = 0
		_Falloff("Falloff", Range( 0.001 , 1)) = 0.001
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _ColorBack;
		uniform float4 _ColorFront;
		uniform float _Offset;
		uniform float _Distance;
		uniform float _Falloff;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float clampResult40 = clamp( ( ( _Offset + ase_worldPos.z ) / _Distance ) , 0 , 1 );
			float4 lerpResult11 = lerp( _ColorBack , _ColorFront , saturate( pow( clampResult40 , _Falloff ) ));
			o.Emission = lerpResult11.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14501
2567;34;2546;1524;1582.932;704.4925;1.08;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;50;-452.7357,211.3484;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;42;-424.9747,105.4285;Float;False;Property;_Offset;Offset;2;0;Create;True;0;0;26.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-359.3628,379.4822;Float;False;Property;_Distance;Distance;3;0;Create;True;0;0;5.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-155.9747,185.4285;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;39;-135.9925,325.9052;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-83.40302,564.1992;Float;False;Property;_Falloff;Falloff;4;0;Create;True;0;0.001;1;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;114.642,156.1304;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;41;303.2426,338.4253;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-337.2385,-283.9739;Float;False;Property;_ColorBack;Color Back;1;0;Create;True;0;0,1,0.7517242,0;0,1,0.7517242,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-335.864,-108.367;Float;False;Property;_ColorFront;Color Front;0;0;Create;True;0;0,1,0.7517242,0;0.8161765,0.06001299,0.06001299,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;32;53.95709,50.6199;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;114.7418,-102.6559;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;49;391.0693,-233.56;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SyntyStudios/DistanceGradient;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;42;0
WireConnection;43;1;50;3
WireConnection;39;0;43;0
WireConnection;39;1;37;0
WireConnection;40;0;39;0
WireConnection;41;0;40;0
WireConnection;41;1;38;0
WireConnection;32;0;41;0
WireConnection;11;0;2;0
WireConnection;11;1;18;0
WireConnection;11;2;32;0
WireConnection;49;2;11;0
ASEEND*/
//CHKSM=D6D0E54A78B486C72B5EC410E123219116FFE5BC