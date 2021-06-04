using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text score_text;
    public Text multiplier_text;

    int required_score;
    float current_score;
    int multiplier;

    float elapsed_time = 0;
    public float multiplier_show_time;
    public float score_increasing_speed;

    // Update is called once per frame
    void Update()
    {
        if(required_score > current_score)
        {
            current_score += score_increasing_speed * Time.deltaTime;
            if (current_score > required_score) current_score = required_score;

            score_text.text = ( (int)current_score ).ToString();
        }

        if(multiplier_text.text != "")
        {
            elapsed_time += Time.deltaTime;
            if (elapsed_time > multiplier_show_time) multiplier_text.GetComponent<Animator>().Play("Disappearing"); //multiplier_text.text = "";
        }
    }

    public void SetToZero()
    {
        current_score = required_score = multiplier = 0;
        
        score_text.text = "0";
        multiplier_text.text = "";
    }

    public void SetMultiplier(int multiplier)
    {
        if (multiplier_text.text == "") multiplier_text.GetComponent<Animator>().Play("Appearing"); 

        this.multiplier = multiplier;
        multiplier_text.text = "x" + multiplier.ToString();
        elapsed_time = 0;
    }

    public void setScore(int score)
    {
        required_score = score;
    }
}
