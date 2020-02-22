using UnityEngine;
using PathCreation;

public class Truck : Unit
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    
    public new void StartIdle() {
        Debug.Log("Truck Idling");
        _movementMode = MovementMode.Move;
    }

    private new void Start() {
        base.Start();
        
        DeathCallback = () => {
            GameObject.Find("GameManager")
                      .GetComponent<WinLossManager>()
                      .ReportTruckDead();
        };

        _movementMode = MovementMode.Move;
        for (int i = 0; i < pathCreator.path.NumPoints; ++i) {
            ShiftMove(pathCreator.path.GetPoint(i), MovementMode.Move);
        }
    }
}
