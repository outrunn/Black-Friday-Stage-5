using System.Collections;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    public string startNode = "IntroScript";    // name of the node to run

    [SerializeField] private GameObject bossSprite;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform tutorialSpawn;
    [SerializeField] private Transform mainSpawn;
    


    void Start()
    {
        player.SetActive(false);

        dialogueRunner.onNodeStart.AddListener(OnNodeStart);
        dialogueRunner.onNodeComplete.AddListener(OnNodeEnd);
        bossSprite.SetActive(false);
        infoPanel.SetActive(false);
    }

    void Update()
    {
    }

    void OnNodeStart(string nodeName)
    {
        if (nodeName == "BossNode")
        {
            // Set boss sprite active
            bossSprite.SetActive(true);
        }
    }
    void OnNodeEnd(string nodeName)
    {
        if (nodeName == "BossNode")
        {
            // Set boss sprite active
            bossSprite.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Tutorial);
            player.transform.position = tutorialSpawn.position;
            player.SetActive(true);
        }
        else if (nodeName == "Tutorial")
        {
            infoPanel.SetActive(false);
            // player.transform.position = mainSpawn.position;
        }
    }


}
