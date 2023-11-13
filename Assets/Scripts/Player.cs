using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// General
	private Platform goalPlatform;
	public Platform GoalPlatform
	{
		get { return goalPlatform; }
		set {
			goalPlatform = value;
			QueuePathCalculation();
			StartCoroutine(FollowPath());
		}
	}
	private World world;

	// Called on intialization
	private void Awake()
	{
		world = FindObjectOfType<World>();
	}

	// Calculate path calculation
	private void QueuePathCalculation()
	{
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit);
		Platform currentPlatform = raycastHit.collider.GetComponentInParent<Platform>(true);
		world.CalculatePath(currentPlatform, GoalPlatform);
	}

	// Move to goal
	private IEnumerator FollowPath()
	{
		yield return null;
	}
}
