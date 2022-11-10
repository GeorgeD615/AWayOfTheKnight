using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private Animator anim;
    public CoinCounter coins;
    public PlayerCollectables _collectables;
    public int levelToLoad;
    public int currentLevel;
    private bool isPlayerDead = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeToLevel()
    {
        anim.SetTrigger("fade");
    }
    public void OnFadeComplete()
    {
        if (isPlayerDead)
        {
            SceneManager.LoadScene(currentLevel);
        }
        else
        {
            coins.coins = _collectables.points;
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void Respawn()
    {
        isPlayerDead = true;
        anim.SetTrigger("fade");
    }

}
