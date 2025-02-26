using System;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health}
    public InfoType type;
    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp=GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = "Level: " + GameManager.instance.level;
                break;
            case InfoType.Kill:
                myText.text = "Kill: " + GameManager.instance.kill;
                break;
            case InfoType.Time:
                myText.text = "Time: " + GameManager.instance.GameTime;
                break;
            case InfoType.Health:
                //mySlider.value = GameManager.instance.player.health;
                break;
        }
    }
}
