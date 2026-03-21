using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject MenuPause;

    public void BackToMenu(){
        SceneManager.LoadScene("Nocty");
    }

    public void PauseGame(){
        Time.timeScale=0; //pauser le temps ? ne fonctionne pas actuellement
        //l'écran devrait empêcher le joueur de bouger des objets
    }

    public void ResumeGame(){
        Time.timeScale=1; //relancer le temps ? ne fonctionne pas actuellement
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuPause.SetActive(false);
    }

}
