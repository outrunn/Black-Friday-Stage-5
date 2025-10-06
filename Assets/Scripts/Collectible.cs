using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CircleCollider2D collider;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the user collided with this object
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); //destroy this object
            GameStateManager.Instance.numOfCollectibles++; //increase the number ofcollectibles by 1
        }
    }
}
