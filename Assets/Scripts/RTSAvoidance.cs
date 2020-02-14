using UnityEngine;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using Pathfinding.Util;
using System.Linq;
using System;

public class RTSAvoidance : MonoBehaviour {
    IAstarAI ai;
    RVOController rvo;

    public Action ReachedDestinationCallback;

    [Range(0, 1)]
    public float densityFraction = 0.5f;
    public bool returnAfterBeingPushedAway = true;

    void Awake() {
        ai = GetComponent<IAstarAI>();
        rvo = GetComponent<RVOController>();
    }

    float timer1 = 0;
    Vector3 prevDestination;
    bool reachedCurrentDestination;

    /// <summary>See https://en.wikipedia.org/wiki/Circle_packing</summary>
    const float MaximumCirclePackingDensity = 0.9069f;
    static List<IAgent> agentBuffer = new List<IAgent>();

    float timer2 = 0;
    bool ShouldStop() {
        // If no destination has been set, then always stop
        if (float.IsPositiveInfinity(ai.destination.x))
            return true;

        var thisAgent = rvo.rvoAgent;
        var radius = (ai.destination - ai.position).magnitude;

        var radius2 = thisAgent.Radius * 5;

        if (radius > radius2) {
            // If the agent is far away from the destination then do a faster check around the destination first.
            if (AgentDensityInCircle(rvo.To2D(ai.destination), radius2) < MaximumCirclePackingDensity * densityFraction)
                return false;
        }

        var result = AgentDensityInCircle(rvo.To2D(ai.destination), radius) > MaximumCirclePackingDensity * densityFraction;
        //Pathfinding.Util.Draw.Debug.CircleXZ(ai.destination, radius, result ? Color.green : Color.red);

        timer2 = Mathf.Lerp(timer2, result ? 1 : 0, Time.deltaTime);
        return result && timer2 > 0.1f;
    }

    float AgentDensityInCircle(Vector2 position, float radius) {
        agentBuffer.ClearFast();
        rvo.simulator.Quadtree.FindAllTouchingCircle(position, radius, agentBuffer);
        var accArea = 0f;

        // Only a single agent, never stop in that case as then the agent should just fall back
        // to whatever the behaviour of the movement script is when this component is not used at all
        if (agentBuffer.Count == 1)
            return 0f;

        for (int i = 0; i < agentBuffer.Count; i++) {
            var agent = agentBuffer[i];
            var distToDestination = (rvo.To2D(ai.destination) - agent.Position).magnitude;
            // This will be greater than 1 if the whole agent is inside the circle with radius #radius
            // and less than 0 if the whole agent is outside the circle.
            // Otherwise it will be between 0 and 1.
            float fractionInside = 0.5f + 0.5f * (radius - distToDestination) / agent.Radius;
            if (fractionInside > 0) {
                accArea += Mathf.Min(1, fractionInside) * agent.Radius * agent.Radius * Mathf.PI;
            }
        }

        return accArea / (radius * radius * Mathf.PI);
    }

    /// <summary>
    /// True if the agent has reached its destination.
    /// If the agents destination changes this will return false until a new path has been calculated.
    /// Note that changing the destination every frame may cause this value to never return true.
    ///
    /// True will be returned if the agent has stopped due to being close enough to the destination.
    /// This may be quite some distance away if there are many other agents around the destination.
    ///
    /// See: <see cref="Pathfinding.IAstarAI.destination"/>
    /// </summary>
    public bool reachedDestination {
        get {
            CheckDestination();
            return reachedCurrentDestination;
        }
    }

    void CheckDestination() {
        if (ai.destination != prevDestination) {
            timer1 = float.PositiveInfinity;
            reachedCurrentDestination = false;
        }
        prevDestination = ai.destination;
    }

    void Update() {
        if (rvo.locked)
            return;

        CheckDestination();

        if (ai.reachedEndOfPath || ShouldStop()) {
            if (!ai.pathPending) {
                timer1 = 0f;
                reachedCurrentDestination = true;
                ReachedDestinationCallback();
            }
            //ai.isStopped = true;
            rvo.flowFollowingStrength = Mathf.Lerp(rvo.flowFollowingStrength, 1.0f, Time.deltaTime * 1);
            if (rvo.flowFollowingStrength > 0.9f)
                ai.isStopped = true;
            rvo.priority = Mathf.Lerp(rvo.priority, 0.05f, Time.deltaTime * 2);
        } else {
            timer1 += Time.deltaTime;

            if (timer1 > 3) {
                if (reachedCurrentDestination) {
                    if (returnAfterBeingPushedAway) {
                        ai.isStopped = false;
                        rvo.flowFollowingStrength = 0f;
                        rvo.priority = Mathf.Lerp(rvo.priority, 0.1f, Time.deltaTime * 2);
                    }
                } else {
                    ai.isStopped = false;
                    rvo.flowFollowingStrength = 0f;
                    rvo.priority = Mathf.Lerp(rvo.priority, 0.5f, Time.deltaTime * 4);
                }
            }
        }
    }
}
