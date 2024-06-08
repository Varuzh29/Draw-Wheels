using DG.Tweening;
using UnityEngine;
using VarCo;

public class RacePage : MonoBehaviour
{
	[SerializeField] private UIButton restartButton;
	[SerializeField] private GameObject tutorial;
	private bool shown = false;
	private Sequence timer;

	private void OnEnable()
	{
		restartButton.OnClick.AddListener(OnRestartButtonClick);
		tutorial.SetActive(GameManager.StorageReader.Level == 1);
	}

	private void Update()
	{
		if (tutorial.activeSelf && Input.GetMouseButtonDown(0))
		{
			tutorial.SetActive(false);
			if (!shown)
			{
				shown = true;
				timer?.Kill();
				timer = DOTimer.Set(5, () =>
				{
					tutorial.SetActive(true);
				});
			}
		}
	}

	private void OnRestartButtonClick()
	{
		GameManager.RestartRace();
	}

	private void OnDisable()
	{
		restartButton.OnClick.RemoveListener(OnRestartButtonClick);
		timer?.Kill();
	}
}
