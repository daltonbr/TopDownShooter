using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

	public AudioClip mainTheme;
	public AudioClip menuTheme;

	string sceneName;

	void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("[MusicManager] A scene name: " + scene.name + " was loaded!");
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

	void PlayMusic()
    {
		AudioClip clipToPlay = null;

		if (sceneName == "Menu")
        {
			clipToPlay = menuTheme;
		} else if (sceneName == "Game")
        {
			clipToPlay = mainTheme;
		}

		if (clipToPlay != null)
        {
			AudioManager.instance.PlayMusic (clipToPlay, 2);
			Invoke ("PlayMusic", clipToPlay.length);
		}

	}

}
