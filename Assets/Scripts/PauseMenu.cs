using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance {get; private set;}


    [SerializeField] GameObject MenuPause;
    [SerializeField] TextMeshProUGUI scoreText;
    bool isPaused;
    
    public void BackToMenu(){
        SceneManager.LoadScene("Nocty");
    }

    public void PauseGame(){
        Time.timeScale=0; //pauser le temps
        isPaused=true;
    }

    public void ResumeGame(){
        Time.timeScale=1; //relancer le temps
        isPaused=false;
    }

    public void LoadGame() {
        //pour le bouton play, lance la scene du jeu
        Time.timeScale=1;
        isPaused=false;
        SceneManager.LoadScene("1stBuild");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuPause.SetActive(false);
    }

    public void YouDied(){

        //affiche l'écran de mort

        //récupère et affiche le score
        int score=DragAndDropController.Instance.score;
        scoreText.text = $"Score: {score}";
    }
    

}
