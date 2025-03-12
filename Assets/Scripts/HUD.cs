using System;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, Gold }
    public InfoType type;

    private Text myText;
    private Slider mySlider;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            originalPosition = rectTransform.localPosition;
        }
    }

    void Start()
    {
        if (rectTransform != null)
        {
            rectTransform.localPosition = originalPosition;
        }
    }

    void Update()
    {
        if (rectTransform != null && type == InfoType.Exp)
        {
            if (rectTransform.localPosition != originalPosition)
            {
                rectTransform.localPosition = originalPosition;
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.instance == null) return;

        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                if (mySlider != null)
                {
                    mySlider.value = curExp / maxExp;
                    Spawn.Instance.Increasenumberofenemy();
                }
                break;
            case InfoType.Level:
                if (myText != null)
                {
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                }
                break;
            case InfoType.Kill:
                if (myText != null)
                {
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                }
                break;
            case InfoType.Time:
                float aliveTime = GameManager.instance.GameTime;
                int min = (int)aliveTime / 60;
                int sec = (int)aliveTime % 60;
                if (myText != null)
                {
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                if (mySlider != null)
                {
                    mySlider.value = curHealth / maxHealth;
                }
                break;
            case InfoType.Gold:
                if (myText != null)
                {
                    myText.text = string.Format("{0:F0}", GameManager.instance.gold);
                    Debug.Log("Updating Gold UI: " + GameManager.instance.gold);
                }
                break;
        }
    }
}
