using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

public class ScreenController : MonoBehaviour {
    private DatabaseReference reference;
    string[] questions;
    string[] answersLeft;
    string[] answersRight;
    int[] scoresLeft;
    int[] scoresRight;
    TextMesh textBox;
    SelectorController ButtonLeft;
    SelectorController ButtonRight;
    TextMesh textAnswerLeft;
    TextMesh textAnswerRight;
    int score;
    int index;

    // Use this for initialization
    void Start() {
        // Set this before calling into the realtime database.
        index = 0;
        questions = new string[1];
        answersLeft = new string[1];
        scoresLeft = new int[1];
        answersRight = new string[1];
        scoresRight = new int[1];
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://taller-vertical-2018.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("Data").Child("ScoreQuestions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("Error, task faulted");

                questions[0] = "DataNotFound";

                answersLeft[0] = "DataNotFound";

                answersRight[0] = "DataNotFound";

                scoresLeft[0] = 0;
                scoresRight[0] = 0;
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Data found");
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                long childrenCount = snapshot.ChildrenCount;
                questions = new string[childrenCount];
                answersLeft = new string[childrenCount];
                answersRight = new string[childrenCount];
                scoresLeft = new int[childrenCount];
                scoresRight = new int[childrenCount];
                Debug.Log("There are " + childrenCount + " questions");
                for (int i = 1; i < childrenCount; i++)
                {
                    DataSnapshot pregunta = snapshot.Child("Question" + i);
                    DataSnapshot statement = pregunta.Child("Statement");
                    DataSnapshot respuestaL = pregunta.Child("Options").Child("OptionA");
                    DataSnapshot respuestaR = pregunta.Child("Options").Child("OptionB");

                    questions[i-1] = statement.Value.ToString();
                    Debug.Log("Data saved: " + questions[i-1]);
                    answersLeft[i-1] = respuestaL.Child("Statement").Value.ToString();
                    Debug.Log("Data saved: " + answersLeft[i - 1]);
                    answersRight[i-1] = respuestaR.Child("Statement").Value.ToString();
                    Debug.Log("Data saved: " + answersRight[i - 1]);

                    int scoreL = 0;
                    Int32.TryParse(respuestaL.Child("Score").Value.ToString(), out scoreL);
                    scoresLeft[i - 1] = scoreL;
                    Debug.Log("Data saved: " + scoresLeft[i - 1]);

                    int scoreR = 0;
                    Int32.TryParse(respuestaR.Child("Score").Value.ToString(), out scoreR);
                    scoresRight[i - 1] = scoreR;
                    Debug.Log("Data saved: "+scoresRight[i - 1]);
                }
                Debug.Log("Data retreived");
            }
        });
        //Initializing elements
        ButtonLeft = transform.GetChild(3).GetChild(0).GetComponent<SelectorController>();
        ButtonRight = transform.GetChild(4).GetChild(0).GetComponent<SelectorController>();
        StartCoroutine("wait");
        textBox = this.transform.GetChild(0).GetComponent<TextMesh>();
        textAnswerLeft = this.transform.GetChild(1).GetComponent<TextMesh>();
        textAnswerRight = this.transform.GetChild(2).GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wait()
    {
        bool passed = true;
        while (passed)
        {
            yield return new WaitForSeconds(2f);
            textBox.text = questions[index];
            textAnswerLeft.text = answersLeft[index];
            textAnswerRight.text = answersRight[index];
            passed = false;
        }

    }

    public void answerRight()
    {
        if (ButtonRight.on)
        {
            changeQuestion();
            score += scoresRight[index-1];
            Debug.Log("Answered Right: "+ scoresRight[index-1]);
            
        }
        
    }

    public void answerLeft()
    {
        if (ButtonLeft.on)
        {        
            changeQuestion();
            Debug.Log("Answered Left: " + scoresLeft[index-1]);
            score += scoresLeft[index-1];
        }
    }

    public void changeQuestion()
    {
        
        index++;
        if (index < questions.Length)
        {
            ButtonLeft.on = false;
            ButtonRight.on = false;
            ButtonLeft.StartCoroutine("Wait");
            ButtonRight.StartCoroutine("Wait");
            textBox.text = questions[index];
            textAnswerLeft.text = answersLeft[index];
            textAnswerRight.text = answersRight[index];
        }else if (index == questions.Length)
        {
            //Submit Score
            //Change Scene
            Debug.Log(score);   

        }
    }
}
