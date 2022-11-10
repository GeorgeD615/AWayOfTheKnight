using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollectables : MonoBehaviour
{
    public int points = 0;
    public LayerMask _treasureLayer;
    public Text _coinsCount;
    public CoinCounter _coinsFromPrevLevel;



    void Start()
    {
        points = _coinsFromPrevLevel.coins;
    }

    void Update()
    {
        _coinsCount.text = "x" + points;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            points += other.GetComponent<Treasure>().points_price;
            other.GetComponent<Treasure>().points_price = 0;
            Destroy(other.gameObject);
        }
    }
}
