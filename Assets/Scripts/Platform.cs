using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	// General
	public List<Platform> adjacentPaths = new List<Platform>();

	// Called before update
	private void Awake()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);
		foreach (Collider collider in colliders)
		{
			Platform platform = collider.GetComponent<Platform>();
			if (platform != null && platform != this) adjacentPaths.Add(platform);
		}
		Debug.Log(adjacentPaths.Count);
	}
}