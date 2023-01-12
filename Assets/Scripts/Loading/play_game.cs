using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///Play Game Button

public class play_game : MonoBehaviour
{
    public void play_game_button() {
        loading.load(loading.Scene.game_world);
    }
}
