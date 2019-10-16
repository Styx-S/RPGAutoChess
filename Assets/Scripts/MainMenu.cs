using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] mainMenuElements;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void quitGame() {
        Application.Quit();
    }

    public void alertAbout() {
        
    }

    private bool visibility = true;
    private void changeVisibility() {
        visibility = !visibility;
        foreach(GameObject gameObject in mainMenuElements) {
            gameObject.SetActive(visibility);
        }
    }

}
