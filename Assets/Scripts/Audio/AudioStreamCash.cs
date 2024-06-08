using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class AudioStreamCash : MonoBehaviour
{
	public static AudioStreamCash Instance;
	public static event Action CashLoaded;

	[SerializeField] private bool Cash;
	[SerializeField] private bool DinamicCash = true;
	[SerializeField] private List<Clip> infoList = new List<Clip>();
	//public AudioClip this[int index] => infoList[index].Cash;

	private int loaded = 0;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

#if UNITY_EDITOR
		ReloadFiles();
#endif

#if !UNITY_EDITOR
    foreach (var item in infoList) item.ClearCash();    
#endif

		if (Cash)
		{
			LoadCash();
		}
	}

	[Button]
	private void ReloadFiles()
	{
		infoList.Clear();
		LoadExt("mp3", AudioType.MPEG);
		LoadExt("wav", AudioType.WAV);
		LoadExt("ogg", AudioType.OGGVORBIS);
	}

	private void LoadExt(string ext, AudioType type)
	{
		DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
		FileInfo[] info = dir.GetFiles("*." + ext);
		foreach (FileInfo item in info)
		{
			infoList.Add(new Clip(Application.streamingAssetsPath, item.Name, ext, type, Cash || DinamicCash));
		}
	}

	private void LoadCash()
	{
		foreach (var item in infoList)
		{
			StartCoroutine(item.GetFile((audioClip) => AddLoaded()));
		}
	}

	private void AddLoaded()
	{
		loaded++;
		if (loaded == infoList.Count)
		{
			CashLoaded?.Invoke();
		}
	}

	public static Clip Find(string name)
	{
		foreach (Clip clip in Instance.infoList)
		{
			if (clip.Name == name) return clip;
		}

		return null;
	}
}

[Serializable]
public class Clip
{
	public string Name;
	public string Path;
	public string Ext;
	public AudioType Type;
	public bool Cashing;
	public AudioClip Cash;

	public bool IsCashing => Cash != null;

	public Clip(string path, string name, string ext, AudioType type, bool cash)
	{
		Ext = ext;
		Name = name.Substring(0, name.Length - Ext.Length - 1);
		Path = path;
		Type = type;
		Cashing = cash;
	}

	public IEnumerator GetFile(Action<AudioClip> action = null)
	{
		if (IsCashing)
		{
			action?.Invoke(Cash);
		}
		else
		{
			string Url = $"{Application.streamingAssetsPath}/{Name}.{Ext}";
			UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(Url, Type);
			request.SendWebRequest();

			while (!request.isDone)
			{
				yield return null;
			}

			AudioClip cash = DownloadHandlerAudioClip.GetContent(request);

			if (Cashing)
			{
				Cash = cash;
				Cash.name = Name;
			}

			action?.Invoke(cash);
		}
	}


	public void ClearCash()
	{
		Cash = null;
	}
}