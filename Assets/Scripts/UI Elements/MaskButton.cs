using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskButton : MonoBehaviour
{
   public Sprite white;
   public Sprite green;

   public GameObject frame;

   void ToWhite()
   {
        frame.GetComponent<Image>().sprite = white;
   }

   void ToGreen()
   {
        frame.GetComponent<Image>().sprite = green;
   }
}
