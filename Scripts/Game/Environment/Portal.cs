using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    [Tooltip("scene for next scene")]
    public string[] sceneNames;
    public string spawnPointName;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            StartCoroutine(NextScene());
        }
    }

    private IEnumerator NextScene()
    {
        //pass spawnPointName to gameManager
        GameManager.instance.PassSpawnPointName(spawnPointName);
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        yield return new WaitForSeconds(0.3f);
        //teleport player- move new scene
        SceneManager.LoadScene(sceneName);
        //save game
        GameManager.instance.SaveState();
        //make sound effect when attack
        collidableAudio.PlayOneShot(triggerSound, 1.0f);
    }
}
