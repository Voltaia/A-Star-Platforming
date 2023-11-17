using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	// Inspector
	public GameObject statusIndicatorPrefab;

	// General
	[NonSerialized] public List<Platform> AdjacentPlatforms = new();
	private Player _player;
	private MeshRenderer[] _meshRenderers;
	private List<Color> _defaultColors = new List<Color>();
	private Color _hoverColor;
	private Color _goalColor;

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
			if (platform != null && platform != this) AdjacentPlatforms.Add(platform);
		}

		// Create status indicator
		statusIndicator = Instantiate(statusIndicatorPrefab, transform.position + Vector3.up, Quaternion.identity, transform);
		SetStatus(Status.Unvisited);

		// Get material
		_meshRenderers = GetComponentsInChildren<MeshRenderer>();
		CacheDefaultColors();
		_hoverColor = new Color(0.25f, 0.25f, 0.25f);
		_goalColor = new Color(0.5f, 0.5f, 0.0f);

		// Repeat for status indicator
		statusMeshRenderer = statusIndicator.GetComponent<MeshRenderer>();
		defaultStatusColor = statusMeshRenderer.material.color;
		pathStatusColor = new Color(1.0f, 1.0f, 0.0f);

		// Get player
		_player = FindObjectOfType<Player>();
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

	// Cache default colors
	private void CacheDefaultColors()
	{
		foreach (MeshRenderer meshRenderer in _meshRenderers)
			foreach (Material material in meshRenderer.materials) _defaultColors.Add(material.color);
	}

	// Reset colors
	private void ResetColors()
	{
		int materialIndex = 0;
		foreach (MeshRenderer meshRenderer in _meshRenderers)
			foreach (Material material in meshRenderer.materials)
			{
				material.color = _defaultColors[materialIndex++];
			}
	}

	// Overlap default colors
	private void OverlapDefaultColors(Color color)
	{
		int materialIndex = 0;
		foreach (MeshRenderer meshRenderer in _meshRenderers)
			foreach (Material material in meshRenderer.materials)
			{
				material.color = _defaultColors[materialIndex++] + color;
			}
	}

	// When mouse enters collider
	private void OnMouseEnter()
	{
		if (this != _player.GoalPlatform) OverlapDefaultColors(_hoverColor);
	}

	// When mouse is over collider
	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (_player.GoalPlatform != null) _player.GoalPlatform.ResetColors();
			OverlapDefaultColors(_goalColor);
			_player.GoalPlatform = this;
		}
	}

	// When mouse exits collider
	private void OnMouseExit()
	{
		if (this != _player.GoalPlatform) ResetColors();
	}
}