using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField] private bool interactable = true;
	[SerializeField] private float disabledAlpha = 0.5f;
	private CanvasGroup canvasGroup;
	public bool Interactable
	{
		get
		{
			return interactable;
		}
		set
		{
			interactable = value;
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
			canvasGroup.interactable = interactable;
			canvasGroup.alpha = interactable ? 1 : disabledAlpha;
		}
	}
	[SerializeField, Min(0)] private float hoverScale = 1.05f, downScale = 0.95f, duration = 0.15f;
	[Space]
	[SerializeField] private UnityEvent onClick;
	public UnityEvent OnClick => onClick;
	private Tween tween;
	private static bool touchscreen = false;
	//

#if UNITY_EDITOR
	private void OnValidate()
	{
		EditorApplication.delayCall += () =>
		{
			Interactable = interactable;
		};
	}
#endif

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!interactable)
		{
			transform.localScale = Vector3.one;
			return;
		}

		onClick?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!touchscreen && Input.touchCount > 0)
		{
			touchscreen = true;
		}

		if (!interactable)
		{
			transform.localScale = Vector3.one;
			return;
		}

		tween = transform.DOScale(downScale, duration);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!interactable)
		{
			transform.localScale = Vector3.one;
			return;
		}

		tween = transform.DOScale(1, duration);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!interactable)
		{
			transform.localScale = Vector3.one;
			return;
		}

		if (touchscreen)
		{
			transform.localScale = Vector3.one;
			return;
		}
		tween = transform.DOScale(hoverScale, duration);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!interactable)
		{
			transform.localScale = Vector3.one;
			return;
		}

		if (touchscreen)
		{
			transform.localScale = Vector3.one;
			return;
		}

		tween = transform.DOScale(1, duration);
	}

	private void OnDestroy()
	{
		tween.Kill();
	}
}
