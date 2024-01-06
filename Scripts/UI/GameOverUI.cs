using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDelliveredText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);

        });
        menuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }


    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += Instance_OnStateChange;

        Hide();
    }

    private void Instance_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipeDelliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
