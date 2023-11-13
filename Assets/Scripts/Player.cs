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
			MoveToGoal();
		}
	}
	private World world;

	// Called on intialization
	private void Awake()
	{
		world = FindObjectOfType<World>();
	}

	// Start followin a path
	private void MoveToGoal()
	{
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit);
		Platform currentPlatform = raycastHit.collider.GetComponentInParent<Platform>(true);
		world.CalculatePath(currentPlatform, GoalPlatform);
	}
}
