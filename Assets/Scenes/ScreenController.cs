using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {
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
        questions = new string[5];
        answersLeft = new string[5];
        answersRight = new string[5];
        textBox = this.transform.GetChild(0).GetComponent<TextMesh>();
        textAnswerLeft = this.transform.GetChild(1).GetComponent<TextMesh>();
        textAnswerRight = this.transform.GetChild(2).GetComponent<TextMesh>();
        index = 0;
        questions[0] = "Pregunta1";
        questions[1] = "Pregunta2";
        questions[2] = "Pregunta3";
        questions[3] = "Pregunta4";
        questions[4] = "Pregunta5";

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

        textBox.text = questions[index].ToString();
        textAnswerLeft.text = answersLeft[index];
        textAnswerRight.text = answersRight[index];
    }
	
	// Update is called once per frame
	void Update () {
		
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
