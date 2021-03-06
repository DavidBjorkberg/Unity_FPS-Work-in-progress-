﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHandler : MonoBehaviour
{
    public WaypointHandler waypointHandler;
    private EnemyMovement movement;
    private EnemyColorController colorController;
    private PlayerDetection playerDetection;
    public enum States { Patrolling, Chasing, Shooting, DetectedPlayer, LookAround, Dead };
    private States state;
    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        colorController = GetComponent<EnemyColorController>();
        playerDetection = GetComponent<PlayerDetection>();
    }
    public void SwitchToPatrolState()
    {
        colorController.SwitchToStandardColour();
        movement.MoveToNextWaypoint();
        state = States.Patrolling;
        movement.navAgent.isStopped = false;
    }
    public void SwitchToDetectedPlayerState()
    {
        colorController.SwitchToAwareColour();
        state = States.DetectedPlayer;
        movement.navAgent.isStopped = true;

    }
    public void SwitchToChasingState()
    {
        colorController.SwitchToAwareColour(true);
        CreateWaypoint(Waypoint.EnemyWaypointBehaviour.LookAround, GameManager.instance.player.transform.position);
        movement.MoveToNextWaypoint();
        state = States.Chasing;
        movement.navAgent.isStopped = false;

    }
    //Helper function to SwitchToChasingState()
    Waypoint CreateWaypoint(Waypoint.EnemyWaypointBehaviour behaviour, Vector3 pos)
    {
        GameObject instantiatedGO = Instantiate(new GameObject());
        instantiatedGO.AddComponent<Waypoint>();
        instantiatedGO.GetComponent<Waypoint>().Initialize(behaviour, pos, true);
        waypointHandler.waypoints.Insert(waypointHandler.curWaypointTargetIndex, instantiatedGO.GetComponent<Waypoint>());
        return instantiatedGO.GetComponent<Waypoint>();
    }
    public void SwitchToShootingState()
    {
        state = States.Shooting;
        movement.navAgent.isStopped = true;
    }
    public void SwitchToDeadState()
    {
        colorController.SwitchToStandardColour();
        state = States.Dead;
        movement.StopRotating();
        movement.navAgent.isStopped = true;
        
    }
    public void SwitchToLookAroundState()
    {
        state = States.LookAround;
        movement.navAgent.isStopped = true;
    }
    public bool IsCurrentState(States isState)
    {
        return state == isState;
    }
    public States GetState()
    {
        return state;
    }
}
