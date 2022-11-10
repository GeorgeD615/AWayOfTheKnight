using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSet : MonoBehaviour
{
    private Animator anim;
    public int sceneToLoad;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeToLevel()
    {
        Debug.Log("Set");
        anim.SetTrigger("fade");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
