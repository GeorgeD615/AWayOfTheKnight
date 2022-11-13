using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSet : MonoBehaviour
{
    private Animator anim;
    public int sceneToLoad;
    public GameObject Player;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeToLevel()
    {
        Player.GetComponent<PlayerMovement>().gameOver = true;
        Player.GetComponent<PlayerCombat>().enabled = false;
        //Player.GetComponent<CharacterController>().enabled = false;

        anim.SetTrigger("fade");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
