using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MusicPlayer.Visual
{
    public class VisualRectangle : MonoBehaviour
    {
        public Vector3 Position
        {
            get
            {
               return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }
        public Vector3 Scale
        {
            get
            {
                return transform.localScale;
            }
            set
            {
                transform.localScale = value;
            }
        }
        public Vector3 Euler
        {
            get
            {
                return transform.eulerAngles;
            }
            set
            {
                transform.eulerAngles = value;
            }
        }
    }
}
