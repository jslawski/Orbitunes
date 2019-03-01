using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : MonoBehaviour {

    public void CloseTutorialScene()
    {
        GameManager.instance.CloseTutorialScene();
    }
}
