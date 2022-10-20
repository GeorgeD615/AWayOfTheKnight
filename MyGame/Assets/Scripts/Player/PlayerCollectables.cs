using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectables : MonoBehaviour
{
    public int points = 0;
    public LayerMask _treasureLayer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            points += other.GetComponent<Treasure>().points_price;
            Destroy(other.gameObject);
        }
    }
}
