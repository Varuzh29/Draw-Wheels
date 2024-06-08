using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;

	public void Show()
    {
        UI.SetActive(true);
    }

    public void Hide()
    {
        UI.SetActive(false);
    }
}
