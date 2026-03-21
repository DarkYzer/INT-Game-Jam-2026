using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] GameObject Settings;
    public void LoadGame() {
        //pour le bouton play, lance la scene du jeu
        //SceneManager.LoadScene("???");
    }

    public void ExitGame(){
        //ferme le jeu correctement ?
        Application.Quit();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
