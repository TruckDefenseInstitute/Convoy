using UnityEngine;
using PathCreation;

public class Truck : Unit
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    
    GameObject _truckSmoke;
    
    public new void StartIdle() {
        Debug.Log("Truck Idling");
        _movementMode = MovementMode.Move;
    }

    private new void Start() {
        base.Start();
        
        DeathCallback = () => {
            WinLossManager.Instance.ReportTruckDead();
        };

        _movementMode = MovementMode.Move;
        for (int i = 0; i < pathCreator.path.NumPoints; ++i) {
            ShiftMove(pathCreator.path.GetPoint(i), MovementMode.Move);
        }

        _truckSmoke = transform.Find("TruckSmoke").gameObject;
    }

    private new void Update()
    {
        base.Update();
        
        if (Health < (MaxHealth / 5))
        {
            _truckSmoke.SetActive(true);
        }
    }
}
