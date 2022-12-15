using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPickUp : MonoBehaviour
{
    [SerializeField] int pointForMaskPickUp = 10;

    bool wasCollected = false;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointForMaskPickUp);
            gameObject.SetActive(false);
            Destroy(gameObject);

        }
        
    }
}
