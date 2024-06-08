using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VarCo;

public class Painter : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private Drawing m_drawing;
    [SerializeField] private Drawing[] playerDrawings;
	[SerializeField] private GameObject drawConstr;
	[SerializeField] private Rigidbody playerRigidbody;
	[SerializeField] private float checkSphereRadius;
	[SerializeField] private LayerMask checkSphereLayerMask;
	[SerializeField] private float drawingCooldown;
	private bool painting = false;
	private Vector3 lastPoint = new Vector3();
	private float lastStartPaintingTime;
	private Sequence timer;

	private void OnEnable()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState gameState)
	{
		if (gameState == GameState.Race)
		{
			m_drawing.transform.position = drawConstr.transform.position + transform.forward * -0.1f;
			m_drawing.transform.rotation = transform.rotation;
		}
		else
		{
			if (painting) StopPainting();
			m_drawing.Clear();
		}
	}

	private void Start()
	{
		playerRigidbody.isKinematic = true;
	}

	public List<RaycastResult> UIRaycast()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results;
	}

	private void StartPainting()
	{
		if (GameManager.GameState != GameState.Race) return;
		if (lastStartPaintingTime + drawingCooldown > Time.unscaledTime) return;

		List<RaycastResult> results = UIRaycast();

		if (results.Count < 1) return;

		if (results[0].gameObject.name != drawConstr.name) return;

		if (painting) return;

		m_drawing.Clear();
		painting = true;
		Time.timeScale = GameManager.Config.SlowMotionTimeScale;
		lastStartPaintingTime = Time.unscaledTime;
	}

	private void StopPainting()
	{
		if (!painting) return;
		painting = false;

		playerRigidbody.isKinematic = false;
		if (m_drawing.LineRenderer.positionCount > 1)
		{
			Vector3[] positions = new Vector3[m_drawing.LineRenderer.positionCount];
			m_drawing.LineRenderer.GetPositions(positions);

			if (IsOverlapping())
			{
				playerRigidbody.position += playerRigidbody.transform.up * 2f;
			}

			timer?.Kill();
			timer = DOTimer.Set(0.1f, () =>
			{
				DrawWheels(positions);
			});
		}

		Time.timeScale = 1f;
		GameManager.EventBus.PlayerStopPainting?.Invoke();
	}

	private bool IsOverlapping()
	{
		bool overlapping = false;
		foreach (var drawing in playerDrawings)
		{
			if (Physics.CheckSphere(drawing.transform.position, checkSphereRadius, checkSphereLayerMask))
			{
				overlapping = true;
				break;
			}
		}
		return overlapping;
	}

	private void DrawWheels(Vector3[] positions)
	{
		foreach (var drawing in playerDrawings)
		{
			drawing.SetLinePositions(positions);
			drawing.GenerateMesh();
		}
	}

	private Vector3 MouseWorldPosition()
	{
		Vector3 position = m_mainCamera.ScreenToWorldPoint(new Vector3
			(
			x: Input.mousePosition.x,
			y: Input.mousePosition.y,
			z: transform.InverseTransformPoint(m_drawing.transform.position).z
			));
		return position;
	}

	private void Painting()
	{
		Vector3 currentPoint = MouseWorldPosition();
		float distance = Vector3.Distance(lastPoint, currentPoint);
		if (distance < GameManager.Config.LineStep) return;

		List<RaycastResult> results = UIRaycast();

		if (results.Count > 0 && results[0].gameObject.name == drawConstr.name)
		{
			m_drawing.AddPoint(currentPoint);
			lastPoint = currentPoint;
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !painting)
		{
			StartPainting();
		}

		if (Input.GetMouseButtonUp(0) && painting)
		{
			StopPainting();
		}

		if (painting) Painting();
	}

	private void OnDisable()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
		timer?.Kill();
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (playerDrawings.Length < 0) return;
		Gizmos.color = Color.yellow;
		foreach (Drawing drawing in playerDrawings)
		{
			if (drawing != null)
			{
				Gizmos.DrawWireSphere(drawing.transform.position, checkSphereRadius);
			}
		}
	}
#endif
}
