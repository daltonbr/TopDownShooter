using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class MusicManager : MonoBehaviour
{

	public AudioClip mainTheme;
	public AudioClip menuTheme;

	string sceneName;

	void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded();
        Assert.IsNotNull(mainTheme, "[MusicManager] mainTheme is null!");
        Assert.IsNotNull(menuTheme, "[MusicManager] menuTheme is null!");
    }

    private void OnSceneLoaded() //Scene scene, LoadSceneMode mode
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("[MusicManager] A scene name: " + scene.name + " was loaded!");
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
