using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioYB : MonoBehaviour
{
	public AudioSource AudioSource => source;
	public string clip { get => source.clip.name; set => source.clip = AudioStreamCash.Find(value).Cash; }

	private AudioSource source;
	private bool load;
	private bool play;
	private bool playLoop;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	public void SetLoop(bool loop)
	{
		Loop(loop);
	}

	private void PlayAfter()
	{
		play = false;
		playLoop = true;
		source.Play();
	}

	private void LoadAfter(AudioClip clip)
	{
		source.clip = clip;
		load = true;

		if (play)
		{
			PlayAfter();
		}
	}

	public void Play()
	{
		if (load)
		{
			source.Play();
			playLoop = true;
		}
		else
		{
			play = true;
		}
	}

	public void Play(string name)
	{
		source.pitch = 1;
		Clip clip = AudioStreamCash.Find(name);

		if (clip == null)
		{
			return;
		}

		load = false;
		play = true;

		StartCoroutine(clip.GetFile(LoadAfter));
	}

	private void LoadShotAfter(AudioClip clip, float volumeScale)
	{
		source.PlayOneShot(clip, volumeScale);
	}

	public void PlayOneShot(string clipName, float volumeScale = 1)
	{
		var clip = AudioStreamCash.Find(clipName);

		if (clip == null)
		{
			return;
		}

		StartCoroutine(clip.GetFile(delegate (AudioClip audioClip) { LoadShotAfter(audioClip, volumeScale); }));
	}


	private void Loop(bool enable)
	{
		if (source.time == 0 && enable && playLoop) 
		{ 
			source.Play();
		}
	}
	public void Pause()
	{
		source.Pause();
	}

	public void UnPause()
	{
		source.UnPause();
	}

	public void Stop()
	{
		playLoop = false;
		source.Stop();
	}
}

