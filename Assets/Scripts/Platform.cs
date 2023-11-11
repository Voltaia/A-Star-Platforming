using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	// General
	[NonSerialized] public List<Platform> adjacentPlatforms = new();

	// Called before update
	private void Awake()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
		foreach (Collider collider in colliders)
		{
			Platform platform = collider.GetComponent<Platform>();
			if (platform != null && platform != this) adjacentPlatforms.Add(platform);
		}
	}

	// Get distance
	public float GetDistanceTo(Platform platform)
	{
		return Vector3.Distance(transform.position, platform.transform.position);
	}
}