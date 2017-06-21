using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour
{

	public enum AudioChannel {Master, Sfx, Music};

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }

    public AudioMixer mixer;
	AudioSource sfx2DSource;
	AudioSource[] musicSources;
	int activeMusicSourceIndex;

	public static AudioManager instance;

	Transform audioListener;
	Transform playerT;

	SoundLibrary library;

	void Awake()
    {
		if (instance != null)
        {
			Destroy (gameObject);
		} else
        {

			instance = this;
			DontDestroyOnLoad (gameObject);

            Assert.IsNotNull(mixer, "[AudioManager] Mixer not found!");

            // Subscribe  a delegate
            SceneManager.sceneLoaded += OnSceneLoaded;

            library = GetComponent<SoundLibrary> ();

            // Instantiante 'i' AudioSource's
            musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++)
            {
				GameObject newMusicSource = new GameObject ("MusicSource" + (i + 1));
				musicSources [i] = newMusicSource.AddComponent<AudioSource> ();
                musicSources [i].outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
                //TODO: why doesn't this is setted?
                musicSources [i].volume = 1f;
				newMusicSource.transform.parent = transform;
            }

			GameObject newSfx2Dsource = new GameObject ("2DSfxSource");
			sfx2DSource = newSfx2Dsource.AddComponent<AudioSource> ();
            sfx2DSource.outputAudioMixerGroup = mixer.FindMatchingGroups("FX")[0];
			newSfx2Dsource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener> ().transform;
			if (FindObjectOfType<Player> () != null)
            {
				playerT = FindObjectOfType<Player> ().transform;
			}

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1.0f);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", 1.0f);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1.0f);

            //masterVolumePercent = 1f;
            //sfxVolumePercent = 1f;
            //musicVolumePercent = 1f;

            //Debug.Log("musicVolumePercent: " + musicVolumePercent);
            //musicSources[0].volume = 1f;
            //musicSources[1].volume = 0.9f;
            //Debug.Log("volume: " + musicSources[1].volume);
        }
	}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("[AudioManager] A scene name: "+ scene.name + " was loaded!");
        if (playerT == null)
        {
            if (FindObjectOfType<Player>() != null)
            {
                playerT = FindObjectOfType<Player>().transform;
            }
        }
    }
    
	void Update()
    {
        // If the player is alive, set the audioListener position to it
		if (playerT != null) {
			audioListener.position = playerT.position;
		}
	}

	public void SetVolume(float volumePercent, AudioChannel channel)
    {
		switch (channel)
        {
		case AudioChannel.Master:
			masterVolumePercent = volumePercent;
			break;
		case AudioChannel.Sfx:
			sfxVolumePercent = volumePercent;
			break;
		case AudioChannel.Music:
			musicVolumePercent = volumePercent;
			break;
		}

		musicSources [0].volume = musicVolumePercent * masterVolumePercent;
		musicSources [1].volume = musicVolumePercent * masterVolumePercent;

		PlayerPrefs.SetFloat ("master vol", masterVolumePercent);
		PlayerPrefs.SetFloat ("sfx vol", sfxVolumePercent);
		PlayerPrefs.SetFloat ("music vol", musicVolumePercent);
		PlayerPrefs.Save ();
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources [activeMusicSourceIndex].clip = clip;
		musicSources [activeMusicSourceIndex].Play ();

		StartCoroutine(AnimateMusicCrossfade(fadeDuration));
	}

	public void PlaySound(AudioClip clip, Vector3 pos)
    {
		if (clip != null)
        {
			AudioSource.PlayClipAtPoint (clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	public void PlaySound(string soundName, Vector3 pos)
    {
		PlaySound (library.GetClipFromName (soundName), pos);
	}

	public void PlaySound2D(string soundName)
    {
		sfx2DSource.PlayOneShot (library.GetClipFromName (soundName), sfxVolumePercent * masterVolumePercent);
	}


	IEnumerator AnimateMusicCrossfade(float duration)
    {
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * 1 / duration;
			musicSources [activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, percent);
			musicSources [1-activeMusicSourceIndex].volume = Mathf.Lerp (musicVolumePercent * masterVolumePercent, 0, percent);
			yield return null;
		}
	}
}
