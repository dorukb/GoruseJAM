using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject screamPanel;
    public AudioSource screamSource;

    bool isGameOver = false;
    bool hasScreamed = false;
    private void Update()
    {
        if (!isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space) && !hasScreamed)
        {
            // start scream
            hasScreamed = true;
            screamSource.loop = false;
            screamSource.Play();
            screamPanel.SetActive(false);
        }
    }
    public void OnGameOver()
    {
        // player enable "space" input
        isGameOver = true;
        // show "Space to Scream" on UI
        screamPanel.SetActive(true);
    }
}
