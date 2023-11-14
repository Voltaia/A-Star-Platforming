using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	// General
	public List<Platform> platformPath = new();
	public bool calculatingPath = false;

	// Settings
	private const float StepTime = 0.25f;

	// Get path from platform A to platform B
	public void CalculatePath(Platform fromPlatform, Platform toPlatform)
	{
		StopAllCoroutines();
		foreach (Platform platform in FindObjectsOfType<Platform>()) platform.SetStatus(Platform.Status.Unvisited);
		platformPath.Clear();
		StartCoroutine(TraversePath(fromPlatform, toPlatform));
		calculatingPath = true;
	}

	// Traverse the path step by step
	private IEnumerator TraversePath(Platform fromPlatform, Platform toPlatform)
	{
		// Initialize
		IndexedPriorityQueue<Platform> priorityQueue = new();
		Dictionary<Platform, float> distancesFromStart = new();
		Dictionary<Platform, Platform> previousPlatforms = new();
		HashSet<Platform> visited = new();

		// Visite platform
		void VisitPlatform(Platform platform, Platform previousPlatform, float distanceFromStart)
		{
			priorityQueue.Push(platform, distanceFromStart);
			distancesFromStart[platform] = distanceFromStart;
			previousPlatforms[platform] = previousPlatform;
			visited.Add(platform);

			platform.SetStatus(Platform.Status.Visited);
		}

		// Visit the start platform
		VisitPlatform(fromPlatform, null, 0.0f);
		yield return new WaitForSeconds(StepTime);

		// Loop through platforms in priority queue
		while (priorityQueue.Count > 0)
		{
			// Pop most promising platform
			Platform poppedPlatform = (Platform)priorityQueue.Pop();

			// Loop through adjacent platforms
			foreach (Platform discoveredPlatform in poppedPlatform.adjacentPlatforms)
			{
				// Get distance
				float discoveredDistance = distancesFromStart[poppedPlatform] + 1;

				// Check if it is in IPQ, otherwise, check if it has been visited
				if (priorityQueue.ContainsKey(discoveredPlatform))
				{
					// Decrease priority and define shorter path
					priorityQueue.DecreasePriority(discoveredPlatform, discoveredDistance);
					distancesFromStart[discoveredPlatform] = discoveredDistance;
					previousPlatforms[discoveredPlatform] = poppedPlatform;
					Debug.Log("Decrease priority, make sure this works if you see this");
				}
				else if (!visited.Contains(discoveredPlatform))
				{
					// Add patform to data structures
					VisitPlatform(discoveredPlatform, poppedPlatform, discoveredDistance);
					yield return new WaitForSeconds(StepTime);
				}

				// Check if this is our goal
				if (toPlatform == discoveredPlatform) goto FoundGoal;
			}
		}

	// Found the goal, now backtrace and return the result
	FoundGoal:
		Platform backtracePlatform = toPlatform;
		while (backtracePlatform != null)
		{
			platformPath.Insert(0, backtracePlatform);
			backtracePlatform.SetStatus(Platform.Status.Path);
			backtracePlatform = previousPlatforms[backtracePlatform];
			yield return new WaitForSeconds(StepTime);
		}
		calculatingPath = false;
	}
}