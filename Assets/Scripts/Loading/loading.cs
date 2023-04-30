using UnityEngine.SceneManagement;


//Not sure if even used in game.

public static class loading {

    public enum Scene {
        game_world,
    }

    
    public static void load(Scene scene){
        SceneManager.LoadSceneAsync(scene.ToString());
    }

}
