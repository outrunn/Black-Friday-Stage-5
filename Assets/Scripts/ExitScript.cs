using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private BoxCollider2D collider;
    [SerializeField] private GameObject infoPanel;

    void Awake()
    {
        infoPanel.SetActive(false);
        collider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
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

    //coroutine to show the infopanel and then hide after 2 seconds
    IEnumerator showAndWaitTwoSeconds()
    {
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        infoPanel.SetActive(false);
    }
}
