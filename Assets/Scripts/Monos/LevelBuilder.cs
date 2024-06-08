using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using VarCo;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField, Min(1)] private int levelSeed = 1;
    private LevelSegment lastSegment;
	private List<LevelSegment> levelSegments = new List<LevelSegment>();
	public event Action LevelBuilt;

	private void InstantiateSegment(LevelSegment levelSegment)
	{
		Vector3 startPosition = lastSegment == null ? transform.position : lastSegment.NextSegmentPosition;
		LevelSegment segment = Instantiate(levelSegment, startPosition, transform.rotation);
		segment.transform.SetParent(transform);
		lastSegment = segment;
	}

	[Button]
    private void Build()
    {
		levelSegments.Clear();
		UnityEngine.Random.State normalState = UnityEngine.Random.state;
		UnityEngine.Random.InitState(levelSeed);
		GameManager.ColorScheme = GameManager.Config.ColorSchemes.GetRandom();

		foreach (LevelSegmentCollection collection in GameManager.Config.LevelSegmentsSet.LevelSegmentCollections)
		{
			if (collection.minLevel <= levelSeed)
			{
				foreach (LevelSegmentConfig segment in collection.segments)
				{
					for (int i = 0; i < segment.frequency; i++)
					{
						levelSegments.Add(segment.prefab);
					}
				}
			}
		}

		Extensions.DestroyImmediateAllChildren(transform);
		InstantiateSegment(GameManager.Config.StartSegment);
		for (int i = 0; i < GameManager.Config.SegmentsCount; i++)
		{
			LevelSegment randomSegment = GetRandomSegment();
			InstantiateSegment(randomSegment);
		}
		InstantiateSegment(GameManager.Config.EndSegment);
		UnityEngine.Random.state = normalState;
		LevelBuilt?.Invoke();
    }

    [Button]
    private void Clear()
    {
		Extensions.DestroyImmediateAllChildren(transform);
	}

	private LevelSegment GetRandomSegment()
	{
		return levelSegments[UnityEngine.Random.Range(0, levelSegments.Count)];
	}

	private void Start()
	{
		if (GameManager.Config.LoadLevelFromPlayerData) levelSeed = GameManager.StorageReader.Level;
		Build();
	}
}
