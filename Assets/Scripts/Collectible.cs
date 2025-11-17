using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CircleCollider2D collider;
    //[SerializeField] private bool tutorialCollectible;

    [SerializeField] private TextMeshProUGUI collectiblesText;

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
            
            if (GameStateManager.Instance.CurrentState == GameState.Playing)
            {
                GameStateManager.Instance.numOfCollectibles++; //increase the number ofcollectibles by 1

                collectiblesText.text = GameStateManager.Instance.numOfCollectibles + "/ 2";  
            }
            else if (GameStateManager.Instance.CurrentState == GameState.Tutorial)
            {
                GameStateManager.Instance.numOfTutorialCollectibles++; //increase the number ofcollectibles by 1

                collectiblesText.text = GameStateManager.Instance.numOfTutorialCollectibles + "/ 1"; 
            }
            
        }
    }
}
