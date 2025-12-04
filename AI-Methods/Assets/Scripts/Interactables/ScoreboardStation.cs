using TMPro;
using UnityEngine;

public class ScoreboardStation : InteractableI
{
    [SerializeField] private TextMeshProUGUI semanticTimeText;
    [SerializeField] private TextMeshProUGUI searchTimeText;
    [SerializeField] private TextMeshProUGUI patternTimeText;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject Scoreboardcanvas;
    
    protected override void Interact()
    {
        if (pauseMenu.isPaused || player.isLoading)
        {
            return;
        }

        if (player.isInteracting)
        {
            player.isInteracting = false;
            Scoreboardcanvas.SetActive(false);
        }
        else
        {
            player.isInteracting = true;
            Scoreboardcanvas.SetActive(true);
            
            float semanticBest = ScoreboardValues.SemanticBestTime;
            float searchBest = ScoreboardValues.SearchBestTime;
            float patternBest = ScoreboardValues.PatternBestTime;

            if (float.IsPositiveInfinity(semanticBest))
            {
                semanticTimeText.text = "Semantische Netze: -";  // no record yet
            }
            else
            {
                semanticTimeText.text = "Semantische Netze: " + semanticBest;
            }
            
            if (float.IsPositiveInfinity(searchBest))
            {
                searchTimeText.text = "Suche: -";  // no record yet
            }
            else
            {
                searchTimeText.text = "Suche: " + searchBest;
            }
            
            if (float.IsPositiveInfinity(patternBest))
            {
                patternTimeText.text = "Musterabgleich: -";  // no record yet
            }
            else
            {
                patternTimeText.text = "Musterabgleich: " + patternBest;
            }
        }
    }
}
