using TMPro;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private PolygonCollider2D polyCollider;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the vi9ewi area collided with the player
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player.lives > 1)
            {
                player.lives--;
            }
            else
            {
                Destroy(collision.gameObject);
                //Set the state to GameOver and open up the game panel
                GameStateManager.Instance.SetState(GameState.GameOver);
            }
            
        }
    }
}
