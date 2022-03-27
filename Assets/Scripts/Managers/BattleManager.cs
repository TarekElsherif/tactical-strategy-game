using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] List<AbstractCommandInput> _players;
    [SerializeField] int _actionsPerUnit;

    Queue<AbstractCommandInput> _activePlayers;
    Dictionary<AbstractCommandInput, ArmyController> _playersToControllers;
    AbstractCommandInput _currentActivePlayer;
    int _turnIndex = -1;

    public System.Action<AbstractCommandInput, ArmyController> OnTurnStarted;
    public System.Action<AbstractCommandInput, ArmyController> OnGameEnd;

    void Awake()
    {
        _playersToControllers = new Dictionary<AbstractCommandInput, ArmyController>();
        _activePlayers = new Queue<AbstractCommandInput>();
        foreach (AbstractCommandInput player in _players)
        {
            _activePlayers.Enqueue(player);
            _playersToControllers[player] = player.GetComponent<ArmyController>();
        }
    }

    void OnEnable()
    {
        foreach (AbstractCommandInput player in _players)
        {
            _playersToControllers[player].OnAllActionsDone += GoToNextTurn;
            _playersToControllers[player].OnAllUnitsDied += CheckForWinner;
        }
    }

    void OnDisable()
    {
        foreach (AbstractCommandInput player in _players)
        {
            _playersToControllers[player].OnAllActionsDone -= GoToNextTurn;
            _playersToControllers[player].OnAllUnitsDied -= CheckForWinner;
        }
    }

    public void StartBattle()
    {
        GoToNextTurn();
    }

    void GoToNextTurn()
    {
        if (_currentActivePlayer != null)
        {
            if (_playersToControllers[_currentActivePlayer].IsAlive)
                _activePlayers.Enqueue(_currentActivePlayer);
            _currentActivePlayer.enabled = false;
            _currentActivePlayer = null;
        }

        if (_activePlayers.Count == 1)
        {
            // WIN CASE
            EndBattle(_activePlayers.Dequeue());
            return;
        }

        _currentActivePlayer = _activePlayers.Dequeue();
        ArmyController armyController = _playersToControllers[_currentActivePlayer];

        if (!armyController.IsAlive)
            GoToNextTurn();

        armyController.SetActionsPerUnits(_actionsPerUnit);
        if (_currentActivePlayer != null)
        {
            _currentActivePlayer.enabled = true;
            if (OnTurnStarted != null)
                OnTurnStarted(_currentActivePlayer, armyController);
        }
    }

    void EndBattle(AbstractCommandInput winner)
    {
        ArmyController wonArmy = _playersToControllers[winner];
        if (OnGameEnd != null)
            OnGameEnd(winner, wonArmy);
        //Debug.Log(wonArmy.ArmyName + " won!");
    }

    void CheckForWinner()
    {
        List<AbstractCommandInput> remainingPlayers = new List<AbstractCommandInput>();
        foreach (AbstractCommandInput player in _players)
        {
            if (_playersToControllers[player].IsAlive)
                remainingPlayers.Add(player);
        }
        if (remainingPlayers.Count == 1)
            EndBattle(remainingPlayers[0]);
    }
}
