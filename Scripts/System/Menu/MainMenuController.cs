using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//for exit
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenuController : MonoBehaviour
{
    public string sceneName;
    private AudioSource mainMenuAudio;
    //    0         1       2
    //naivagate - cancel - reset
    public AudioClip[] mainMenuMusic;

    private void Start()
    {
        mainMenuAudio = GetComponent<AudioSource>();
    }
    //start game
    public void StartGame(string name)
    {
        PlaySoundNavigate();
        StartCoroutine(InStartGame(name));
    }
    //coroutine inStartGame
    private IEnumerator InStartGame(string name)
    {
        yield return new WaitForSeconds(0.1f);
        PassSceneName(name);
        //Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
        GameManager.instance.OnStartGame();
    }
    //exit game
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    //reset game
    public void ResetGame()
    {
        GameManager.instance.ResetSaveState();
    }

    //get sceneName from other
    public void PassSceneName(string name)
    {
        sceneName = name;
        //Debug.Log(sceneName);
    }
    //play sound
    public void PlaySoundNavigate()
    {
        mainMenuAudio.PlayOneShot(mainMenuMusic[0], 1.0f);
    }
    public void PlaySoundCancel()
    {
        mainMenuAudio.PlayOneShot(mainMenuMusic[1], 1.0f);
    }
    public void PlaySoundReset()
    {
        mainMenuAudio.PlayOneShot(mainMenuMusic[2], 1.0f);
    }
}
