using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class loading {

    public enum Scene {
        game_world,
    }

    
    public static void load(Scene scene){
        SceneManager.LoadSceneAsync(scene.ToString());
    }

}
