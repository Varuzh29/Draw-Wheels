using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class LevelSegmentCollection
{
	[Min(1)] public int minLevel = 1;
	public List<LevelSegmentConfig> segments = new List<LevelSegmentConfig>();
}

[Serializable]
public class LevelSegmentConfig
{
	public LevelSegment prefab;
	[Min(1)] public int frequency = 1;
}

[CreateAssetMenu]
public class LevelSegmentsSet : ScriptableObject
{
	[SerializeField] private List<LevelSegmentCollection> levelSegmentCollections = new List<LevelSegmentCollection>();
	public IEnumerable<LevelSegmentCollection> LevelSegmentCollections => levelSegmentCollections;
}
