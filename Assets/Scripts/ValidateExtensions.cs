using UnityEngine;

public static class ValidateExtensions
{
	public static void ValidateInspectorField(this MonoBehaviour monoBehaviour, object target)
	{
		if (target == null)
		{
			UnityEngine.Debug.LogError(monoBehaviour.name + ": Is missing component", monoBehaviour);
		}
	}
}
