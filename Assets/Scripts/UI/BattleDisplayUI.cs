using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleDisplayUI : MonoBehaviour
{
    [SerializeField] BattleManager _battleManager;
    [SerializeField] TMPro.TextMeshProUGUI _turnText;
    [SerializeField] TMPro.TextMeshProUGUI _gameoverText;
    [SerializeField] GameObject _startPanel;
    [SerializeField] GameObject _gameoverPanel;

    void Start()
    {
        _gameoverPanel.SetActive(false);
        _startPanel.SetActive(true);
    }

    void OnEnable()
    {
        _battleManager.OnTurnStarted += UpdateTurnDisplay;
        _battleManager.OnGameEnd += DisplayGameOver;
    }

    void OnDisable()
    {
        _battleManager.OnTurnStarted -= UpdateTurnDisplay;
        _battleManager.OnGameEnd -= DisplayGameOver;
    }

    public void StartBattle()
    {
        _battleManager.StartBattle();
        _startPanel.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateTurnDisplay(AbstractCommandInput input, ArmyController _army)
    {
        _turnText.text = _army.ArmyName + "'s Turn";
    }

    void DisplayGameOver(AbstractCommandInput input, ArmyController _army)
    {
        _gameoverPanel.SetActive(true);
        _gameoverText.text = _army.ArmyName + " won the battle!";
    }
}
