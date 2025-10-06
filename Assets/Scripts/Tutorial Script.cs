using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText; //instructions
    [SerializeField] private GameObject collectable; //image of the object that the user needs 
                                                     //to collect

    [SerializeField] private float timeInbetweenText = 2f;

    void Start()
    {
        tutorialText.text = ""; //make sure that the text is blank
        collectable.SetActive(false); 

        StartCoroutine(TutorialRoutine()); //starts the coroutine
    }

    //coroutine that switches the instructions text and waits 2 seconds inbetween each text change
    IEnumerator TutorialRoutine()
    {
        tutorialText.text = "Escape the store";

        yield return new WaitForSeconds(timeInbetweenText); //wait 

        tutorialText.text = "Find the 2 collectibles";
        collectable.SetActive(true); //set the object to be visible so the user knows 
                                     //what they need to collect

        yield return new WaitForSeconds(timeInbetweenText); //wait

        collectable.SetActive(false); //set the object to be invisible

        tutorialText.text = "You have 5 minutes to find the collectibles and escape";

        yield return new WaitForSeconds(timeInbetweenText); //wait

        tutorialText.text = "Good Luck";

        GameStateManager.Instance.SetState(GameState.Playing); //start the game and the timer
    }
}
