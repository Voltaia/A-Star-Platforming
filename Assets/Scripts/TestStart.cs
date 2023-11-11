using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStart : MonoBehaviour
{
	private void Start()
	{
		Platform startPlatform = GetComponent<Platform>();
		Platform endPlatform = FindObjectOfType<TestEnd>().GetComponent<Platform>();
		World world = FindObjectOfType<World>();
		world.CalculatePath(startPlatform, endPlatform);
	}
}
