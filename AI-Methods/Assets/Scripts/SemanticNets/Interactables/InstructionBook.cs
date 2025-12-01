using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InstructionBook : InteractableI
{
    [SerializeField]
    public GameObject panel;
    [SerializeField]
    public GameObject canvasToDisable;
    private bool active = false;
    private Button buttonForward;
    private Button buttonBackward;
    private TextMeshProUGUI textPanel;
    private string[] pages = {
        "Hier wurde einmal das gesamte Wissen der KI gesammelt. Leider sind die Verbindungen verloren gegangen und die KI versteht die Zusammenhänge nicht mehr. Hilf der KI, wieder ein qualitatives Wissen herzustellen!",
        "Wissen kann in sogenannten semantischen Netzen dargestellt werden. Knoten repräsentieren Konzepte, während Kanten die Beziehungen zwischen diesen Konzepten darstellen.",
        "Das Ergebnis des ersten Levels hat eine Baumstruktur, mit einem 'Wurzel'-Knoten und mehreren 'Blatt'-Knoten. Vorsicht: Falsche Verbindungen werden von der KI als Fehler gewertet und ziehen Punktabzug nach sich!",
        "Denk daran: Je mehr richtige Verbindungen du herstellst, desto besser versteht die KI die Welt um sie herum! Nur du kannst sie reparieren."
    };
    private int currentPage = 0;
    private TextMeshProUGUI pageCountPanel;
    
    public static InstructionBook Instance;

    void Awake()
    {
        Instance = this;
        panel.SetActive(active);
        textPanel = panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textPanel.text = pages[currentPage];
        buttonForward = panel.transform.GetChild(1).GetComponent<Button>();
        buttonForward.onClick.AddListener(ButtonForwardClicked);
        buttonBackward = panel.transform.GetChild(2).GetComponent<Button>();
        buttonBackward.onClick.AddListener(ButtonBackwardClicked);
        pageCountPanel = panel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        pageCountPanel.text = "Seite " + (currentPage + 1) + " von " + pages.Length;
    }


    protected override void Interact()
    {
        alreadyInteracted = true;
        active = !active;
        if (active) {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        currentPage = 0;
        textPanel.text = pages[currentPage];
        pageCountPanel.text = "Seite " + (currentPage + 1) + " von " + pages.Length;
        panel.SetActive(active);
        canvasToDisable.SetActive(!active);
    }

    private void ButtonForwardClicked()
    {
        currentPage++;
        if (currentPage >= pages.Length) {
            Interact();
            return;
        }
        textPanel.text = pages[currentPage];
        pageCountPanel.text = "Seite " + (currentPage + 1) + " von " + pages.Length;
    }

    private void ButtonBackwardClicked()
    {
        currentPage--;
        if (currentPage < 0) {
            Interact();
            return;
        }
        textPanel.text = pages[currentPage];
        pageCountPanel.text = "Seite " + (currentPage + 1) + " von " + pages.Length;
    }
}
