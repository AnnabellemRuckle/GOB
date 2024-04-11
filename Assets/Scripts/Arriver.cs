using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pretty messy - since the arriver script inherits from the kinematic script which inherits 
//from monobehavior i chose to put the other behaviors in here to just switch them in the inspector depending on the object

public class Arriver : Kinematic
{
    public enum SteeringType
    {
        Face,
        Arrive,
        Wander,
        Separation
    }

    public SteeringType steeringBehavior;
    Face myFaceMoveType;
    Arrive myArriveMoveType;
    Wander myWanderMoveType;
    Separation mySeparationMoveType;
    Align myRotateType;
    GameObject wanderTarget; 
    // Start is called before the first frame update
    void Start()
    {
        myFaceMoveType = new Face();  
        myFaceMoveType.character = this;
        myFaceMoveType.target = myTarget;

        myArriveMoveType = new Arrive();
        myArriveMoveType.character = this;
        myArriveMoveType.target = myTarget;

        myWanderMoveType = new Wander();
        myWanderMoveType.character = this;

        mySeparationMoveType = new Separation();
        mySeparationMoveType.character = this;
        mySeparationMoveType.targets = GetSeparationTargets();

        myRotateType = new Align();
        myRotateType.character = this;
        myRotateType.target = myTarget;

        //setting the initial wander target
        myWanderMoveType.character.myTarget = GetRandomWanderTarget();
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();

        //for selected behaviro
        switch (steeringBehavior)
        {
            case SteeringType.Face:
                steeringUpdate.linear = myFaceMoveType.getSteering().linear;
                break;

            case SteeringType.Arrive:
                steeringUpdate.linear = myArriveMoveType.getSteering().linear;
                break;

            case SteeringType.Wander:
                steeringUpdate.linear = myWanderMoveType.getSteering().linear;
                break;

            case SteeringType.Separation:
                steeringUpdate.linear = mySeparationMoveType.getSteering().linear;
                break;

            default:
                Debug.LogError("NOOOOOOOOO");
                break;
        }

        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }

    private Kinematic[] GetSeparationTargets()
    {
        return GameObject.FindObjectsOfType<Kinematic>();
    }

    private GameObject GetRandomWanderTarget()
    {
        GameObject[] wanderTargets = GameObject.FindGameObjectsWithTag("WanderTarget");

        if (wanderTargets.Length > 0)
        {
            return wanderTargets[Random.Range(0, wanderTargets.Length)];
        }

        return null;
    }
}
