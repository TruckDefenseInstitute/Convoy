using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITutorialTextManager
{
    void Activate();

    void Deactivate();

    /// Returns true if the tutorial section should end
    bool ClickAnywhere();

    /// Returns true if the tutorial section should end
    bool TrainButtonPress();
}
