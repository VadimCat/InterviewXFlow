using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Можно не выдумывать велосипед, а заиспольновать уже готовые решения, например, SrDebugger

//Это скорее CheatView/CheatButton
public class CheatElementBehaviour : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;

    public void Setup(CheatActionDescription description)
    {
        _text.text = description.name;
        _button.onClick.AddListener(() => description.cheatAction());
    }
}

public class CheatManager
{
    // Это скорее просто Cheat
    public class CheatActionDescription
    {
        public readonly string name;
        public readonly Action cheatAction;

        public CheatActionDescription(string name, Action cheatAction)
        {
            this.name = name;
            this.cheatAction = cheatAction;
        }
    }

    public interface ICheatProvider
    {
        IEnumerable<CheatActionDescription> GetCheatActions();
    }

    public static readonly CheatManager Instance = new CheatManager();

    private readonly List<ICheatProvider> _providers = new List<ICheatProvider>();

    private GameObject _panelPrefab;
    private CheatElementBehaviour _cheatElementPrefab;

    private GameObject _panel;

    //Можно передавать в конструкторе
    public void Setup(GameObject panelPrefab, CheatElementBehaviour cheatElementPrefab)
    {
        _panelPrefab = panelPrefab;
        _cheatElementPrefab = cheatElementPrefab;
    }

    public void RegProvider(ICheatProvider provider)
    {
        _providers.Add(provider);
    }

    // Смешана логика отображения и добавления читов, следует разделить на два объекта
    public void ShowCheatPanel()
    {
        if (_panel != null)
            return;

        _panel = UnityEngine.Object.Instantiate(_panelPrefab);
        foreach (var provider in _providers)
        {
            foreach (var cheatAction in provider.GetCheatActions())
            {
                var element = UnityEngine.Object.Instantiate(_cheatElementPrefab, _panel.transform);

                element.Setup(cheatAction);
            }
        }
    }

    public void HideCheatPanel()
    {
        if (_panel == null)
            return;

        UnityEngine.Object.Destroy(_panel);
        _panel = null;
    }
}

// Более лучший подход по сравнений с Example4_2, т.к. занимается только добавлением нужных читов, без привязки к отображению и тд.
// Более подходящее имя HealthCheatProvider
public class SomeManagerWithCheats : CheatManager.ICheatProvider
{
    //Health почему-то находится внутри чита, хотя это скорее атрибут какой-то внутриигровой сущности, а не чита
    //Следует передавать объект Health в конструкторе
    private int _health;
    // Расширение методом Setup нарушает L из soLid,
    // т.к. же добавляет лишнюю статическую зависимость, можно обойтись текущим интерфейсом ICheatProvider
    public void Setup()
    {
        CheatManager.Instance.RegProvider(this);
    }

    IEnumerable<CheatManager.CheatActionDescription> CheatManager.ICheatProvider.GetCheatActions()
    {
        yield return new CheatManager.CheatActionDescription("Cheat health", () => _health++);
        yield return new CheatManager.CheatActionDescription("Reset health", () => _health = 0);
    }
}
