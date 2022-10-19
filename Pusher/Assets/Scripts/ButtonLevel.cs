using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private int scene, preLvTotal, level;

    public int[] GetButton()
    {
        return new int[] { scene, preLvTotal, level };
    }
}
