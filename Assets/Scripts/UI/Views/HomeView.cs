using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    [SerializeField] private Button BoutiqueGameButton; 
    [SerializeField] private Button settingsButton; 


    public override void Init() 
    {
        base.Init();

        // Ajouter des listeners aux boutons
        BoutiqueGameButton.onClick.AddListener(OnStartGameClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);

        // Assurez-vous que la vue commence correctement configurée

    }

    public override void Show(bool isActive)
    {
        base.Show(isActive);
        gameObject.SetActive(isActive);


    }

    public override void Destroy()
    {
        base.Destroy();

        // Retirer les listeners pour éviter les fuites mémoire
        BoutiqueGameButton.onClick.RemoveListener(OnStartGameClicked);
        settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    // Appelé lorsqu'on clique sur "Start Game"
    private void OnStartGameClicked()
    {
        Debug.Log("Start Game clicked!");

        // Transition vers la vue de jeu
        ViewManager.Instance.Show<PlayerHUDView>(true); 
        ViewManager.Instance.Show<HomeView>(false);
    }

    // Appelé lorsqu'on clique sur "Settings"
    private void OnSettingsClicked()
    {
        Debug.Log("Settings clicked!");

        // Transition vers la vue des paramètres
        ViewManager.Instance.Show<PausedView>(true);
        ViewManager.Instance.Show<HomeView>(false);
    }
}