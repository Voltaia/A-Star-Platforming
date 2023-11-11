using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	// Inspector
	public GameObject statusIndicatorPrefab;

	// General
	[NonSerialized] public List<Platform> adjacentPlatforms = new();
	private GameObject statusIndicator;

	// Status types
	public enum Status
	{
		Unvisited,
		Visited
	}

	// Called before update
	private void Awake()
	{
		// Collect adjacent platforms
		Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
		foreach (Collider collider in colliders)
		{
			Platform platform = collider.GetComponent<Platform>();
			if (platform != null && platform != this) adjacentPlatforms.Add(platform);
		}

		// Create status indicator
		statusIndicator = Instantiate(statusIndicatorPrefab, transform.position + Vector3.up * 2.0f, Quaternion.identity, transform);
		SetStatus(Status.Unvisited);
	}

	// Set status
	public void SetStatus(Status status)
	{
		switch (status)
		{
			case Status.Unvisited:
				statusIndicator.SetActive(false);
				break;

			case Status.Visited:
				statusIndicator.SetActive(true);
				break;
		}
	}

	// Get distance
	public float GetDistanceTo(Platform platform)
	{
		return Vector3.Distance(transform.position, platform.transform.position);
	}
}