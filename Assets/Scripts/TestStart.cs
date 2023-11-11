using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStart : MonoBehaviour
{
	private void Start()
	{
		Platform startPlatform = GetComponent<Platform>();
		Platform endPlatform = FindObjectOfType<TestEnd>().GetComponent<Platform>();
		List<Platform> platformPath = World.GetPath(startPlatform, endPlatform);
		Debug.Log(platformPath.Count);
		string stringPath = string.Empty;
		foreach (Platform platform in platformPath)
		{
			stringPath += $"[{platform.name}]";
		}
		Debug.Log(stringPath);
	}
}
