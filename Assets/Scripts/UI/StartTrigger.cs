using UnityEngine;
using UnityEngine.EventSystems;

public class StartTrigger : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		GameManager.StartOpponentSearch();
	}
}
