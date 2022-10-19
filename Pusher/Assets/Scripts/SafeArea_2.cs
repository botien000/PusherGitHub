using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SafeArea_2 : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransfGUI;

    private RectTransform rectTransf;
    private Image img;
    public void SetAnchor(float value)
    {
        img = GetComponent<Image>();
        img.color = Color.black;
        rectTransf = GetComponent<RectTransform>();
        rectTransf.anchorMin = Vector2.up * value;

    }
}
