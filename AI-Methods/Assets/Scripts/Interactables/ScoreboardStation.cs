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
                semanticTimeText.text = "Semantic Nets: --:--";  // no record yet
            }
            else
            {
                int minutes = Mathf.FloorToInt(semanticBest / 60f);
                int seconds = Mathf.FloorToInt(semanticBest % 60f);
                semanticTimeText.text = $"Semantic Nets: {minutes:00}:{seconds:00}";
            }
            
            if (float.IsPositiveInfinity(searchBest))
            {
                searchTimeText.text = "Search: --:--";  // no record yet
            }
            else
            {
                int minutes = Mathf.FloorToInt(searchBest / 60f);
                int seconds = Mathf.FloorToInt(searchBest % 60f);
                searchTimeText.text = $"Search: {minutes:00}:{seconds:00}";
            }
            
            if (float.IsPositiveInfinity(patternBest))
            {
                patternTimeText.text = "Pattern Matching: --:--";  // no record yet
            }
            else
            {
                int minutes = Mathf.FloorToInt(patternBest / 60f);
                int seconds = Mathf.FloorToInt(patternBest % 60f);
                patternTimeText.text = $"Pattern Matching: {minutes:00}:{seconds:00}";
            }
        }
    }
}
