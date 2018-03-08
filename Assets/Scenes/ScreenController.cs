using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour {
    private DatabaseReference reference;
    public AudioClip[] arrayAudios;
    private AudioSource audioSource;
    string age;
    string gender;
    string lifestyle;
    string responsible;
    string[] questions;
    string[] answersLeft;
    string[] answersRight;
    int[] scoresLeft;
    int[] scoresRight;
    string[] playerAnswersL;
    string[] playerAnswersR;
    TextMesh textBox;
    SelectorController ButtonLeft;
    SelectorController ButtonRight;
    TextMesh textAnswerLeft;
    TextMesh textAnswerRight;
    int score;
    int index;
    long subjectQuestionSize = 0;

    // Use this for initialization
    void Start() {
        // Set this before calling into the realtime database.
        index = 0;
        questions = new string[1];
        answersLeft = new string[1];
        scoresLeft = new int[1];
        answersRight = new string[1];
        scoresRight = new int[1];
        playerAnswersL = new string[1];
        playerAnswersR = new string[1];
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://taller-vertical-2018.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        long totalQuestionSize = 0;
        FirebaseDatabase.DefaultInstance.GetReference("Data").Child("ScoreQuestions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                totalQuestionSize += snapshot.ChildrenCount;
                Debug.Log("Question Size: " + totalQuestionSize);
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("Data").Child("SubjectQuestions").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Data found");
                DataSnapshot snapshot = task.Result;
                subjectQuestionSize = snapshot.ChildrenCount;
                playerAnswersL = new string[subjectQuestionSize];
                playerAnswersR = new string[subjectQuestionSize];
                totalQuestionSize += subjectQuestionSize;
                questions = new string[totalQuestionSize];
                answersLeft = new string[totalQuestionSize];
                answersRight = new string[totalQuestionSize];
                scoresLeft = new int[totalQuestionSize];
                scoresRight = new int[totalQuestionSize];
                for(int i = 0; i < subjectQuestionSize; i++)
                {
                    Debug.Log("Yupiii!!1!1!!11");
                    questions[i] = snapshot.Child("Question" + i).Child("Statement").Value.ToString();
                    Debug.Log("Yupiii!!1!1!!11");
                    answersLeft[i] = snapshot.Child("Question" + i).Child("Options").Child("a").Value.ToString();
                    playerAnswersL[i] = answersLeft[i];
                    Debug.Log("Yupiii!!1!1!!11");
                    answersRight[i] = snapshot.Child("Question" + i).Child("Options").Child("b").Value.ToString();
                    playerAnswersR[i] = answersRight[i];
                    Debug.Log("Yupiii!!1!1!!11");
                    scoresLeft[i] = 0;
                    scoresRight[i] = 0;
                    Debug.Log("i: " + i);
                }
                Debug.Log("Yupiii!!1!1!!11");
                this.readScoreQuestions();
            }
        });
        //Initializing elements
        ButtonLeft = transform.GetChild(3).GetChild(0).GetComponent<SelectorController>();
        ButtonRight = transform.GetChild(4).GetChild(0).GetComponent<SelectorController>();
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine("wait");
        textBox = this.transform.GetChild(0).GetComponent<TextMesh>();
        textAnswerLeft = this.transform.GetChild(1).GetComponent<TextMesh>();
        textAnswerRight = this.transform.GetChild(2).GetComponent<TextMesh>();
    }

    private void readScoreQuestions()
    {
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
                Debug.Log("There are " + childrenCount + " questions");
                Debug.Log("Viniendo de: " + (subjectQuestionSize));
                for (int i = 1; i <= childrenCount; i++)
                {
                    DataSnapshot pregunta = snapshot.Child("Question" + i);
                    DataSnapshot statement = pregunta.Child("Statement");
                    DataSnapshot respuestaL = pregunta.Child("Options").Child("OptionA");
                    DataSnapshot respuestaR = pregunta.Child("Options").Child("OptionB");

                    questions[i + subjectQuestionSize - 1] = statement.Value.ToString();
                    Debug.Log("Data saved: " + questions[i + subjectQuestionSize - 1]);
                    answersLeft[i + subjectQuestionSize - 1] = respuestaL.Child("Statement").Value.ToString();
                    Debug.Log("Data saved: " + answersLeft[i + subjectQuestionSize - 1]);
                    answersRight[i + subjectQuestionSize - 1] = respuestaR.Child("Statement").Value.ToString();
                    Debug.Log("Data saved: " + answersRight[i + subjectQuestionSize - 1]);

                    int scoreL = 0;
                    Int32.TryParse(respuestaL.Child("Score").Value.ToString(), out scoreL);
                    scoresLeft[i + subjectQuestionSize - 1] = scoreL;
                    Debug.Log("Data saved: " + scoresLeft[i + subjectQuestionSize - 1]);

                    int scoreR = 0;
                    Int32.TryParse(respuestaR.Child("Score").Value.ToString(), out scoreR);
                    scoresRight[i + subjectQuestionSize - 1] = scoreR;
                    Debug.Log("Data saved: " + scoresRight[i + subjectQuestionSize - 1]);
                }
                Debug.Log("Data retreived");
            }
        });
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(index);
	}

    IEnumerator wait()
    {
        bool passed = true;
        while (passed)
        {
            audioSource.PlayOneShot(arrayAudios[0]);
            yield return new WaitForSeconds(7);
            passed = false;
        }
        textBox.text = questions[index];
        textAnswerLeft.text = answersLeft[index];
        textAnswerRight.text = answersRight[index];
        audioSource.PlayOneShot(arrayAudios[1]);

    }

    public void answerRight()
    {
        if (ButtonRight.on)
        {
            if (index < subjectQuestionSize)
            {
                switch (index)
                {
                    case 0:
                        //Female
                        age = playerAnswersR[index];
                        audioSource.PlayOneShot(arrayAudios[3]);
                        break;
                    case 1:
                        //Adult
                        gender = playerAnswersR[index];
                        System.Random rnd = new System.Random();
                        audioSource.PlayOneShot(arrayAudios[rnd.Next(7, 9)]);
                        break;
                    case 2:
                        //Life gives me less than necessary
                        lifestyle = playerAnswersR[index];
                        audioSource.PlayOneShot(arrayAudios[2]);
                        break;
                    case 3:
                        //No, not responsible
                        responsible = playerAnswersR[index];
                        audioSource.PlayOneShot(arrayAudios[16]);
                        break;
                }
            }
            changeQuestion();
            score += scoresRight[index-1];
            Debug.Log("Answered Right: "+ scoresRight[index-1]);
            
        }
        
    }

    public void answerLeft()
    {
        if (ButtonLeft.on)
        {
            if (index - 1 < subjectQuestionSize)
            {
                switch (index)
                {
                    case 0:
                        //Male
                        age = playerAnswersL[index];
                        audioSource.PlayOneShot(arrayAudios[2]);
                        break;
                    case 1:
                        //Young
                        gender = playerAnswersL[index];
                        System.Random rnd = new System.Random();
                        audioSource.PlayOneShot(arrayAudios[rnd.Next(5, 7)]);
                        break;
                    case 2:
                        //Life gives more than necessary
                        lifestyle = playerAnswersL[index];
                        System.Random random = new System.Random();
                        audioSource.PlayOneShot(arrayAudios[random.Next(11, 13)]);
                        break;
                    case 3:
                        //Yes, im responsible
                        responsible = playerAnswersL[index];
                        audioSource.PlayOneShot(arrayAudios[15]);
                        break;
                }
            }
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
            switch (index)
            {
                case 1:
                    //Ask about age
                    audioSource.PlayOneShot(arrayAudios[4]);
                    break;
                case 2:
                    //Ask about lifestyle
                    audioSource.PlayOneShot(arrayAudios[9]);
                    break;
                case 3:
                    //Ask about responsible
                    audioSource.PlayOneShot(arrayAudios[13]);
                    audioSource.PlayOneShot(arrayAudios[14]);
                    break;
            }
        }
        else if (index == questions.Length)
        {
            //Submit Score
            this.writeNewUser(gender, age, lifestyle, responsible, score.ToString());
            //Change Scene
            if (score > 7)
            {
                SceneManager.LoadScene("Good_Park");
            }
            else
            {
                SceneManager.LoadScene("Bad_Park");
            }
            Debug.Log(score);
        }
    }
    private void writeNewUser(string gender, string age, string lifestyle, string responsible, string score)
    {
        User user = new User(gender, age, lifestyle, responsible, score);
        string json = JsonUtility.ToJson(user);
        string key = FirebaseDatabase.DefaultInstance.GetReference("Data").Child("Users").Push().Key;
        FirebaseDatabase.DefaultInstance.GetReference("Data").Child("Users").Child(key).SetRawJsonValueAsync(json);
    }
}

class User
{
    public string gender;
    public string age;
    public string lifestyle;
    public string responsible;
    public string score;

    public User()
    {}

    public User(string gender, string age, string lifestyle, string responsible, string score)
    {
        this.gender = gender;
        this.age = age;
        this.lifestyle = lifestyle;
        this.responsible = responsible;
        this.score = score;
    }
}
