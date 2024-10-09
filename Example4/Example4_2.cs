using System;
using UnityEngine;
using UnityEngine.UI;

//Это скорее CheatView/CheatButton
public class CommonCheat : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;

    public void Setup(string name, Action cheatAction)
    {
        _text.text = name;
        _button.onClick.AddListener(() => cheatAction());
    }
}

public class CheatManager
{
    public static readonly CheatManager Instance = new CheatManager();

    private GameObject _panel;

    public GameObject Panel => _panel;

    public void Setup(GameObject panel)
    {
        _panel = panel;
        _panel.SetActive(false);
    }

    public void ShowCheatPanel()
    {
        _panel.SetActive(true);
    }

    public void HideCheatPanel()
    {
        _panel.SetActive(false);
    }
}

// Лишний MonoBehaviour, может только добавить читы, но удалить их никак нельзя
public class SomeManagerWithCheats : MonoBehaviour
{
    [SerializeField] private CheatManager.CommonCheat _cheatPrefab;

    private int _health;

    public void Setup()
    {
        var cheat1 = Instantiate(_cheatPrefab, CheatManager.Instance.Panel.transform);
        cheat1.Setup("Cheat health", () => _health++);
        var cheat2 = Instantiate(_cheatPrefab, CheatManager.Instance.Panel.transform);
        cheat2.Setup("Reset health", () => _health = 0);
    }
}
