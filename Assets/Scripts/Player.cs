using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// General
	private Platform _goalPlatform;
	public Platform GoalPlatform
	{
		get { return _goalPlatform; }
		set {
			_goalPlatform = value;
			QueuePathCalculation();
			StopAllCoroutines();
			StartCoroutine(FollowPath());
		}
	}
	private World _world;
	private Rigidbody _rigidbody;
	private Quaternion _desiredRotation;

	// Settings
	private const float JumpForce = 2.5f;
	private const float DirectionalForce = 2.0f;
	private const float JumpWaitTime = 0.6f;

	// Called on intialization
	private void Awake()
	{
		_world = FindObjectOfType<World>();
		_rigidbody = GetComponent<Rigidbody>();
		_desiredRotation = transform.rotation;
	}

	// Called every physics frame
	private void FixedUpdate()
	{
		Quaternion nextRotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 0.15f);
		_rigidbody.MoveRotation(nextRotation);
	}

	// Calculate path calculation
	private void QueuePathCalculation()
	{
		_world.CalculatePath(GetCurrentPlatform(), GoalPlatform);
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
		yield return new WaitWhile(() => _world.CalculatingPath);
		
		foreach (Platform nextPlatform in _world.PlatformPath)
		{
			Platform currentPlatform = GetCurrentPlatform();
			while (currentPlatform != nextPlatform)
			{
				// Jump
				yield return new WaitForFixedUpdate();
				Vector3 direction = (nextPlatform.transform.position - transform.position).normalized;
				Vector3 jumpDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
				Vector3 jumpForce = (jumpDirection * DirectionalForce + Vector3.up * JumpForce);
				_rigidbody.AddForce(jumpForce, ForceMode.Impulse);
				_desiredRotation = Quaternion.LookRotation(jumpDirection);

				// Wait to land and check current platform
				yield return new WaitForSeconds(JumpWaitTime);
				currentPlatform = GetCurrentPlatform();
			}
		}

	}
}
