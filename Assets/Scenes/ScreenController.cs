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
    TextMesh textAnswerLeft;
    TextMesh textAnswerRight;
    int score;
    int index;

    // Use this for initialization
    void Start() {
        // Set this before calling into the realtime database.
        index = 0;
        questions = new string[5];
        answersLeft = new string[5];
        answersRight = new string[5];
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://taller-vertical-2018.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("Data").Child("ScoreQuestions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("Error, task faulted");

                questions[0] = "DataNotFound";
                questions[1] = "DataNotFound";
                questions[2] = "DataNotFound";
                questions[3] = "DataNotFound";
                questions[4] = "DataNotFound";

                answersLeft[0] = "A";
                answersLeft[1] = "A1";
                answersLeft[2] = "Si";
                answersLeft[3] = "Lel";
                answersLeft[4] = "A";

                answersRight[0] = "B";
                answersRight[1] = "B1";
                answersRight[2] = "B2";
                answersRight[3] = "B3";
                answersRight[4] = "B4";
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Task completed");
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
                    Debug.Log(questions[i-1]);
                }
                Debug.Log("Acabado");
                Debug.Log(questions[0]);
            }
        });
        //Initializing elements
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
        score--;
        changeQuestion();
    }

    public void answerLeft()
    {
        score++;
        changeQuestion();
    }

    public void changeQuestion()
    {
        index++;
        if (index < questions.Length)
        {
            Debug.Log("AnsweredRight");
            textBox.text = questions[index];
            textAnswerLeft.text = answersLeft[index];
            textAnswerRight.text = answersRight[index];
        }
    }
}
