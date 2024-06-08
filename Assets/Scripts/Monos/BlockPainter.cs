using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BlockPainter : MonoBehaviour
{
    [SerializeField] private bool darkBlock;
	private MaterialPropertyBlock propertyBlock;
	private Renderer blockRenderer;

#if UNITY_EDITOR
	private void OnValidate()
	{
		blockRenderer = GetComponent<Renderer>();
		propertyBlock = new MaterialPropertyBlock();
		propertyBlock.SetColor("_Color", darkBlock ? GameManager.Config.ColorSchemes.Collection[2].DarkBlockColor : GameManager.Config.ColorSchemes.Collection[2].LightBlockColor);
		blockRenderer.SetPropertyBlock(propertyBlock);
	}
#endif

	private void Awake()
	{
		blockRenderer = GetComponent<Renderer>();
		propertyBlock = new MaterialPropertyBlock();
	}

	private void Start()
	{
		SetColor();
	}

	private void SetColor()
    {
		propertyBlock.SetColor("_Color",darkBlock ? GameManager.ColorScheme.DarkBlockColor :  GameManager.ColorScheme.LightBlockColor);
		blockRenderer.SetPropertyBlock(propertyBlock);
    }
}
