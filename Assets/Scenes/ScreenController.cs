using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {
    ArrayList questions;
    TextMesh textBox;
    int score;
    int index;
	// Use this for initialization
	void Start () {
        textBox = GetComponentInChildren<TextMesh>();
        questions = new ArrayList();
        index = 0;
        questions.Add("Pregunta1");
        questions.Add("Pregunta2");
        questions.Add("Pregunta3");
        
        textBox.text = questions[index].ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void answerRight()
    {
        score++;
        changeQuestion();
    }

    public void answerLeft()
    {
        score-- ;
        changeQuestion();
    }

    public void changeQuestion()
    {
        index++;
        if (index < questions.Count)
        {
            Debug.Log("AnsweredRight");
            textBox.text = questions[index].ToString();
        }
    }
}
