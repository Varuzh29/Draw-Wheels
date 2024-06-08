using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour
{
    private GameObject skin;

	public void SetSkin(int skinId)
	{
		if (skin != null) Destroy(skin);

		GameObject prefab = GameManager.Config.Skins.Collection[skinId].gfxPrefab;
		skin = Instantiate(prefab, transform);
		skin.transform.localPosition = Vector3.zero;
		skin.transform.localRotation = Quaternion.identity;
		skin.transform.localScale = Vector3.one;
	}
}
