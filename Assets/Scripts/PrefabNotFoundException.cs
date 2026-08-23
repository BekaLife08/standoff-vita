using System;

public class PrefabNotFoundException : InvalidOperationException
{
	public PrefabNotFoundException(string path)
		: base("Prefab not found (" + path + ")")
	{
	}
}
