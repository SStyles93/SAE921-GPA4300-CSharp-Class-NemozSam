using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName="interface", menuName = "ScriptableObjects/PlayerManagerInterface")]
public class PlayerManagerInterface : ScriptableObject
{
    UnityAction<PlayerGameLogic> _damageReportAction;

    public void AddDamageReportAction(UnityAction<PlayerGameLogic> action)
    {
        _damageReportAction += action;
    }

    /// <summary>
    /// Report that some damage has been taken by a player
    /// </summary>
    /// <param name="player">the player that has taken damage</param>
    public void ReportDamage(PlayerGameLogic player)
    {
        _damageReportAction.Invoke(player);
    }
}
