using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CircleCollider2D collider;
    [SerializeField] private TextMeshProUGUI collectiblesText;

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with this object
        if (collision.CompareTag("Player"))
        {
            // Instead of destroying the key, simply hide it
            gameObject.SetActive(false);

            if (GameStateManager.Instance.CurrentState == GameState.Playing)
            {
                GameStateManager.Instance.numOfCollectibles++;
                
                if (GameStateManager.Instance.currentLevel == 1)
                {
                    collectiblesText.text = GameStateManager.Instance.numOfCollectibles + "/ 2";
                }
                else if (GameStateManager.Instance.currentLevel == 2)
                {
                    collectiblesText.text = GameStateManager.Instance.numOfCollectibles + "/ 4";
                }
            }
            else if (GameStateManager.Instance.CurrentState == GameState.Tutorial)
            {
                GameStateManager.Instance.numOfTutorialCollectibles++;
                collectiblesText.text = GameStateManager.Instance.numOfTutorialCollectibles + "/ 1";
            }
        }
    }
}
