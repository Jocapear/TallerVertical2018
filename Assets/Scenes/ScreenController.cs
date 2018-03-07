using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class ScreenController : MonoBehaviour {
    private DatabaseReference reference;
    string[] questions;
    string[] answersLeft;
    string[] answersRight;
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
        answersRight = new string[1];
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
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Data found");
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                long childrenCount = snapshot.ChildrenCount;
                questions = new string[snapshot.ChildrenCount];
                answersLeft = new string[snapshot.ChildrenCount];
                answersRight = new string[snapshot.ChildrenCount];
                Debug.Log("There are " + snapshot.ChildrenCount + " questions");
                for (int i = 1; i < snapshot.ChildrenCount; i++)
                {
                    string pregunta = "Question";
                    DataSnapshot value = snapshot.Child(pregunta + i).Child("Statement");
                    questions[i-1] = value.Value.ToString();
                    //Debug.Log(questions[i-1]);
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
            yield return new WaitForSeconds(0.1f);
            textBox.text = questions[index];
            //textAnswerLeft.text = answersLeft[index];
            //textAnswerRight.text = answersRight[index];
            passed = false;
        }

    }

    public void answerRight()
    {
        if (ButtonRight.on)
        {
            score--;
            Debug.Log("Answered Right");
            changeQuestion();
        }
        
    }

    public void answerLeft()
    {
        if (ButtonLeft.on)
        {
            score++;
            Debug.Log("Answered Left");
            changeQuestion();
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

        }
    }
}
