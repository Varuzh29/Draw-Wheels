using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SkinSetter skinSetter;

	private void Awake()
	{
		skinSetter.SetSkin(GameManager.StorageReader.CurrentSkin);
		GameManager.CurrentSkinChanged += OnCurrentSkinChanged;
	}

	private void OnCurrentSkinChanged(int skinId)
	{
		skinSetter.SetSkin(skinId);
	}

	private void OnDestroy()
	{
		GameManager.CurrentSkinChanged -= OnCurrentSkinChanged;
	}
}
