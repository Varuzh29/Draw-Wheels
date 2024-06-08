using DG.Tweening;
using UnityEngine;
using VarCo;

public class BotAI : MonoBehaviour
{
	[SerializeField] private Drawing[] botDrawings;
	[SerializeField] private Rigidbody botRigidbody;
	[SerializeField] private float checkSphereRadius;
	[SerializeField] private LayerMask checkSphereLayerMask;
	[SerializeField] private Participants participants;
	[SerializeField] private float aiTicRate;
	[SerializeField] private float slowdownTicRate;
	[SerializeField] private Pattern[] patterns;
	[SerializeField] private Pattern[] slowPatterns;
	[SerializeField] private float velocityThreshold;
	[SerializeField] private SkinSetter skinSetter;
	[SerializeField] private float slowdownEffect = 4;
	private float lastXVelocity = 0;
	private bool started = false;
	private Sequence timer;
	private bool slowdown = false;

	private void Awake()
	{
		GameManager.GameStateChanged += OnGameStateChanged;
		skinSetter.SetSkin(Random.Range(0, GameManager.Config.Skins.Collection.Length));
	}

	private void OnEnable()
	{
		botRigidbody.isKinematic = true;
		GameManager.EventBus.PlayerStopPainting.AddListener(OnPlayerStopPainting);
	}

	private void OnGameStateChanged(GameState gameState)
	{
		gameObject.SetActive(gameState == GameState.Race || gameState == GameState.Win || gameState == GameState.Lose);
		enabled = gameState == GameState.Race;
	}

	private void OnPlayerStopPainting()
	{
		if (!started)
		{
			ChangeWheels();
			InvokeRepeating(nameof(AITick), 0, aiTicRate);
			InvokeRepeating(nameof(SlowdownTic), 0, slowdownTicRate);
			started = true;
		}
	}

	private void FixedUpdate()
	{
		if (slowdown)
		{
			botRigidbody.AddForce(Vector3.left * slowdownEffect * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}

	private void SlowdownTic()
	{
		slowdown = !participants.IsPlayerLeading();
	}

	private void AITick()
	{
		if (botRigidbody.velocity.x < velocityThreshold)
		{
			ChangeWheels();
		}
		else
		{
			if (botRigidbody.velocity.x < lastXVelocity)
			{
				ChangeWheels(!participants.IsPlayerLeading());
			}
		}

		lastXVelocity = botRigidbody.velocity.x;
	}

	private bool IsOverlapping()
	{
		bool overlapping = false;
		foreach (var drawing in botDrawings)
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
		foreach (var drawing in botDrawings)
		{
			drawing.SetLinePositions(positions);
			drawing.GenerateMesh();
		}
	}

	private void ChangeWheels(bool useBadPatterns = false)
	{
		botRigidbody.isKinematic = false;

		Vector3[] points = useBadPatterns ? slowPatterns[Random.Range(0, slowPatterns.Length)].Points : patterns[Random.Range(0, patterns.Length)].Points;

		for (int i = 0; i < points.Length; i++)
		{
			points[i].z = 0;
		}

		if (IsOverlapping())
		{
			botRigidbody.position += botRigidbody.transform.up * 2f;
		}

		timer?.Kill();
		timer = DOTimer.Set(0.1f, () =>
		{
			DrawWheels(points);
		});
	}

	private void OnDisable()
	{
		CancelInvoke();
		GameManager.EventBus.PlayerStopPainting.RemoveListener(OnPlayerStopPainting);
		timer?.Kill();
	}

	private void OnDestroy()
	{
		GameManager.GameStateChanged -= OnGameStateChanged;
	}
}
