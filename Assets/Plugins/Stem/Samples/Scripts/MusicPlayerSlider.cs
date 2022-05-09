using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SliderAxis
{
	X,
	Y,
	Z,
}

public class MusicPlayerSlider : MonoBehaviour
{
	[Stem.MusicPlayerID]
	public Stem.ID musicPlayer = Stem.ID.None;

	[Range(0.0f, 1.0f)]
	public float value = 0.0f;
	public SliderAxis axis = SliderAxis.X;

	public Camera raycastCamera = null;
	public GameObject body = null;

	private Stem.MusicPlayer musicPlayerInstance = null;
	private Mesh bodyMesh = null;
	private Bounds bodyMeshBounds;
	private bool dragging = false;

	private void Awake()
	{
		if (body)
		{
			MeshFilter bodyMeshFilter = body.GetComponent<MeshFilter>();
			bodyMesh = bodyMeshFilter.sharedMesh;
			bodyMeshBounds = bodyMesh.bounds;

			Vector3 size = bodyMeshBounds.size;
			size.x *= body.transform.localScale.x;
			size.y *= body.transform.localScale.y;
			size.z *= body.transform.localScale.z;

			bodyMeshBounds.size = size;
		}
	}

	private void Start()
	{
		musicPlayerInstance = Stem.MusicManager.GetMusicPlayer(musicPlayer);

		if (musicPlayerInstance != null)
			value = musicPlayerInstance.Volume;
	}

	private void Update()
	{
		if (raycastCamera == null)
			return;

		Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (dragging && Physics.Raycast(ray, out hit))
		{
			Vector3 delta = body.transform.InverseTransformDirection(hit.point - body.transform.position);

			float newValue = 0.0f;
			float size = 0.0f;
			switch (axis)
			{
				case SliderAxis.X: newValue = delta.x; size = bodyMeshBounds.size.x; break;
				case SliderAxis.Y: newValue = delta.y; size = bodyMeshBounds.size.y; break;
				case SliderAxis.Z: newValue = delta.z; size = bodyMeshBounds.size.z; break;
			}

			value = Mathf.Clamp01(newValue / size + 0.5f);
		}

		UpdateSliderPosition();

		if (musicPlayerInstance != null)
			musicPlayerInstance.Volume = value;
	}

	private void UpdateSliderPosition()
	{
		if (body == null)
			return;

		Vector3 sliderPosition = body.transform.localPosition;

		switch (axis)
		{
			case SliderAxis.X: sliderPosition.x += value - 0.5f; break;
			case SliderAxis.Y: sliderPosition.y += value - 0.5f; break;
			case SliderAxis.Z: sliderPosition.z += value - 0.5f; break;
		}

		transform.position = body.transform.TransformPoint(sliderPosition);
	}

	private void OnMouseDown()
	{
		dragging = true;
	}

	private void OnMouseUp()
	{
		dragging = false;
	}
}
