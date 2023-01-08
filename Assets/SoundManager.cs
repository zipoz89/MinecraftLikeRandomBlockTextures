using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    
    public static SoundManager Instance {
        get {
            if (instance == null) {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
            }
            return instance;
        }
    }
    
    
}
