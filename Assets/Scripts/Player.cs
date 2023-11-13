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
			StopAllCoroutines();
			StartCoroutine(FollowPath());
		}
	}
	private World world;
	private new Rigidbody rigidbody;

	// Settings
	private const float JumpForce = 4.0f;
	private const float DirectionalForce = 2.5f;

	// Called on intialization
	private void Awake()
	{
		world = FindObjectOfType<World>();
		rigidbody = GetComponent<Rigidbody>();
	}

	// Calculate path calculation
	private void QueuePathCalculation()
	{
		world.CalculatePath(GetCurrentPlatform(), GoalPlatform);
	}

	// Below platform
	private Platform GetCurrentPlatform()
	{
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit);
		return raycastHit.collider.GetComponentInParent<Platform>(true);
	}

	// Move to goal
	private IEnumerator FollowPath()
	{
		yield return new WaitWhile(() => world.calculatingPath);
		
		foreach (Platform nextPlatform in world.platformPath)
		{
			Platform currentPlatform = GetCurrentPlatform();
			while (currentPlatform != nextPlatform)
			{
				// Jump
				yield return new WaitForFixedUpdate();
				Vector3 direction = (nextPlatform.transform.position - transform.position).normalized;
				Vector3 jumpForce = (direction * DirectionalForce + Vector3.up * JumpForce);
				rigidbody.AddForce(jumpForce, ForceMode.Impulse);

				// Wait to land and check current platform
				yield return new WaitForSeconds(0.5f);
				currentPlatform = GetCurrentPlatform();
			}
		}

	}
}
