using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text questionDisplayText;
    public Text scoreDisplayText;

    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;

    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;
    }

    private void ShowQuestion()    //show questions on QuestionPanel
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];    //load from questions saved in DataController
        questionDisplayText.text = questionData.questionText;

        for (int i = 0; i < questionData.answers.Length; i++)    //using vertical layout group to divide answer button equally according to how many answers are there in one question
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()    //remove previous answer buttons
    {
        while (answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)    //if the answer is correct, add one piont to playerScore
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();
        }

        if (questionPool.Length > questionIndex + 1)    //show next question
        {
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            if(playerScore < 2)    //if player answer two or more questions wrong, trun into GameOverPanel
            {
                SceneManager.LoadScene("GameOver");
            }
            else
                EndRound();
        }

    }

    public void EndRound()    //end current round of quiz
    {
        isRoundActive = false;

        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()    //return to the main game scene
    {  
        //PlayerControl.heart_num++;
        MaskCountScript.maskAmount = 0;
        PatientScript.interacted = false;
        SceneManager.LoadScene("MainScene");
        PlayerControl.health = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
