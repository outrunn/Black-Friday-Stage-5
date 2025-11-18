using System.Collections;
using System.Data.Common;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the view area collided with the player
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            
            if (!player.takeDamage)
            {
                Debug.Log("Cannot Take Damage right now");
                return;
            }
            if (player.lives > 1)
            {
                Debug.Log("You took damage during " + GameStateManager.Instance.CurrentState);
                player.lives -= 1;
                player.TakeDamage();
            }
            else
            {
                //Destroy(collision.gameObject);
                //Set the state to GameOver and open up the game panel
                GameStateManager.Instance.SetState(GameState.GameOver);

            }

        }
    }



    
}
