using UnityEngine;



public class close_menu : MonoBehaviour
{
    public GameObject canvas;
    public GameObject tutorial_button;
    private AudioSource source;
    


    public void canvas_close_tutorial()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        canvas.SetActive(false);
        tutorial_button.GetComponent<open_menu>().state = "closed";    
    }

    public void canvas_close()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        canvas.SetActive(false);
    }
}