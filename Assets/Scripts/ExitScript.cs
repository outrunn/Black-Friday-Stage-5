using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private BoxCollider2D collider;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform mainSpawn;

    void Awake()
    {
        infoPanel.SetActive(false);
        collider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            //if the user leaves and has collected both collectibles
            if (collision.CompareTag("Player") && GameStateManager.Instance.numOfCollectibles == 2)
            {
                GameStateManager.Instance.SetState(GameState.Victory);
            }
            else if (collision.CompareTag("Player") && GameStateManager.Instance.numOfCollectibles != 2)
            {
                StartCoroutine(showAndWaitTwoSeconds());
            }
        }
        else if (GameStateManager.Instance.CurrentState == GameState.Tutorial)
        {
            //teleport player to main map
            if (collision.CompareTag("Player") && GameStateManager.Instance.numOfTutorialCollectibles == 1)
            {
                player.transform.position = mainSpawn.position;
                //GameStateManager.Instance.SetState(GameState.Playing);
                StartCoroutine(startGame());
            }
            else if (collision.CompareTag("Player") && GameStateManager.Instance.numOfTutorialCollectibles != 1)
            {
                StartCoroutine(showAndWaitTwoSeconds());
            }
            
        }
        
    }

    //coroutine to show the infopanel and then hide after 2 seconds
    IEnumerator showAndWaitTwoSeconds()
    {
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        infoPanel.SetActive(false);
    }

    IEnumerator startGame()
    {
        countDownPanel.SetActive(true);
        countDownText.text = "3";

        yield return new WaitForSeconds(1f);

        countDownText.text = "2";

        yield return new WaitForSeconds(1f);

        countDownText.text = "1";

        yield return new WaitForSeconds(1f);

        countDownPanel.SetActive(false);

        GameStateManager.Instance.SetState(GameState.Playing);

    }
}
