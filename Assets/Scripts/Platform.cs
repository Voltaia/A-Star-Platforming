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
	private Player player;
	private MeshRenderer meshRenderer;
	private Color defaultColor;
	private Color hoverColor;
	private Color goalColor;

	// Status indicator
	private GameObject statusIndicator;
	private MeshRenderer statusMeshRenderer;
	private Color defaultStatusColor;
	private Color pathStatusColor;

	// Status types
	public enum Status
	{
		Unvisited,
		Visited,
		Path
	}

	// Called before update
	private void Awake()
	{
		// Collect adjacent platforms
		Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
		foreach (Collider collider in colliders)
		{
			Platform platform = collider.GetComponentInParent<Platform>(true);
			if (platform != null && platform != this) adjacentPlatforms.Add(platform);
		}

		// Create status indicator
		statusIndicator = Instantiate(statusIndicatorPrefab, transform.position + Vector3.up, Quaternion.identity, transform);
		SetStatus(Status.Unvisited);

		// Get material
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		defaultColor = meshRenderer.material.color;
		hoverColor = defaultColor + new Color(0.25f, 0.25f, 0.25f);
		goalColor = defaultColor + new Color(0.5f, 0.5f, 0.0f);

		// Repeat for status indicator
		statusMeshRenderer = statusIndicator.GetComponent<MeshRenderer>();
		defaultStatusColor = statusMeshRenderer.material.color;
		pathStatusColor = new Color(1.0f, 1.0f, 0.0f);

		// Get player
		player = FindObjectOfType<Player>();
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
				statusMeshRenderer.material.color = defaultStatusColor;
				break;

			case Status.Path:
				statusIndicator.SetActive(true);
				statusMeshRenderer.material.color = pathStatusColor;
				break;
		}
	}

	// Get distance
	public float GetDistanceTo(Platform platform)
	{
		return Vector3.Distance(transform.position, platform.transform.position);
	}

	// When mouse enters collider
	private void OnMouseEnter()
	{
		if (this != player.GoalPlatform)
		{
			meshRenderer.material.color = hoverColor;
		}
	}

	// When mouse is over collider
	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (player.GoalPlatform != null) player.GoalPlatform.meshRenderer.material.color = player.GoalPlatform.defaultColor;
			meshRenderer.material.color = goalColor;
			player.GoalPlatform = this;
		}
	}

	// When mouse exits collider
	private void OnMouseExit()
	{
		if (this != player.GoalPlatform) meshRenderer.material.color = defaultColor;
	}
}