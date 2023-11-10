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

	// Get path to platform
	public List<Platform> GetPathTo(Platform goalPlatform)
	{
		// --- Dijkstra's ---

		// Initialize
		IndexedPriorityQueue<Platform> priorityQueue = new();
		Dictionary<Platform, float> distanceFromStart = new();
		Dictionary<Platform, Platform> previousPlatforms = new();
		HashSet<Platform> visited = new();

		// Start here
		priorityQueue.Push(this, 0);
		distanceFromStart[this] = 0;
		previousPlatforms[this] = null;
		visited.Add(this);

		// Loop through platforms in priority queue
		while (priorityQueue.Count > 0)
		{
			// Pop most promising platform
			Platform poppedPlatform = (Platform)priorityQueue.Pop();

			// Loop through adjacent platforms
			foreach (Platform discoveredPlatform in poppedPlatform.adjacentPlatforms)
			{
				// Get distance
				float discoveredDistance = distanceFromStart[poppedPlatform] + 1;

				// Check if it is in IPQ, otherwise, check if it has been visited
				if (priorityQueue.ContainsKey(discoveredPlatform))
				{
					// Decrease priority and define shorter path
					priorityQueue.DecreasePriority(discoveredPlatform, discoveredDistance);
					distanceFromStart[discoveredPlatform] = discoveredDistance;
					previousPlatforms[discoveredPlatform] = poppedPlatform;
				}
				else if (!visited.Contains(discoveredPlatform))
				{
					// Add patform to data structures
					distanceFromStart[discoveredPlatform] = discoveredDistance;
					previousPlatforms[discoveredPlatform] = poppedPlatform;
					priorityQueue.Push(discoveredPlatform, discoveredDistance);
					visited.Add(discoveredPlatform);
				}

				// Check if this is our goal
				if (goalPlatform == discoveredPlatform) goto FoundGoal;
			}
		}

	// Found the goal, now backtrace and return the result
	FoundGoal:
		List<Platform> platformPath = new();
		Platform backtracePlatform = goalPlatform;
		while (backtracePlatform != null)
		{
			platformPath.Insert(0, backtracePlatform);
			backtracePlatform = previousPlatforms[backtracePlatform];
		}
		return platformPath;
	}
}