using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public static List<Platform> GetPath(Platform fromPlatform, Platform toPlatform)
	{
		// Initialize
		IndexedPriorityQueue<Platform> priorityQueue = new();
		Dictionary<Platform, float> distanceFromStart = new();
		Dictionary<Platform, Platform> previousPlatforms = new();
		HashSet<Platform> visited = new();

		// Start here
		priorityQueue.Push(fromPlatform, 0);
		distanceFromStart[fromPlatform] = 0;
		previousPlatforms[fromPlatform] = null;
		visited.Add(fromPlatform);

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
				if (toPlatform == discoveredPlatform) goto FoundGoal;
			}
		}

	// Found the goal, now backtrace and return the result
	FoundGoal:
		List<Platform> platformPath = new();
		Platform backtracePlatform = toPlatform;
		while (backtracePlatform != null)
		{
			platformPath.Insert(0, backtracePlatform);
			backtracePlatform = previousPlatforms[backtracePlatform];
		}
		return platformPath;
	}

	// Traverse the path step by step
	private IEnumerator TraversePath(Platform fromPlatform, Platform toPlatform)
	{
		yield return null;
	}
}
