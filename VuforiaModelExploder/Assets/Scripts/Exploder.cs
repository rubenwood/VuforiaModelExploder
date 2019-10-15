using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Exploder : MonoBehaviour
{
    public Transform ModelRoot;
    public Transform StartLoc;
    public Transform EndLoc;
    public float StartRadius;
    public float EndRadius;
    public float speed;

    private Collider[] cols;
    private List<Vector3> starts = new List<Vector3>();
    private List<Vector3> dests = new List<Vector3>(); 

    // Start is called before the first frame update
    void Start()
    {
        cols = GetColliders();
        starts = GetStarts(cols);
        dests = GetRandPosSphere(cols);
    }

    // Update is called once per frame
    void Update()
    {
        /// Hold A to transform to destination
        if (Input.GetKey(KeyCode.A))
        {
            explode(cols);
        }
        ///Hold S to transform parts to start pos
        if (Input.GetKey(KeyCode.S))
        {
            returnToStartPos(cols);
        }
    }

    void explode(Collider[] colliders)
    {
        int i = 0;
        foreach (Collider col in colliders)
        {
            /// set the destination of this part, using random dest generated at start
            Vector3 dest = (EndLoc.position + (dests[i] * EndRadius));
            ///Move the part toward that destination
            col.transform.position = Vector3.MoveTowards(col.transform.position, dest, speed / 1000);
            i++;
        }
    }

    /// Return all the parts to the starting location 
    /// WORLD CENTER mode MUST be set to SPECIFIC IMAGE for this to work...
    void returnToStartPos(Collider[] colliders)
    {
        int i = 0;
        foreach (Collider col in colliders)
        {
            col.transform.position = Vector3.MoveTowards(col.transform.position, starts[i], speed / 1000);
            i++;
        }
    }

    /// Get all colliders in start radius
    Collider[] GetColliders()
    {        
        Collider[] colliders = Physics.OverlapSphere(StartLoc.position, StartRadius);
        return colliders;
    }
    /// Get all start positions
    List<Vector3> GetStarts(Collider[] colliders)
    {
        List<Vector3> startPositions = new List<Vector3>();

        foreach(Collider col in colliders)
        {
            startPositions.Add(col.transform.position);
        }

        return startPositions;
    }
    ///Get a list of destinations for each collider
    List<Vector3> GetRandPosSphere(Collider[] colliders)
    {
        List<Vector3> RandomPositions = new List<Vector3>();

        foreach(Collider col in colliders)
        {
            RandomPositions.Add((Random.insideUnitSphere * EndRadius));
        }

        return RandomPositions;
    }

    ///Gizmo Helper, draws debug gizmos in scene view
    private void OnDrawGizmos()
    {
        ///Draw sphere gizmos to visualize area
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartLoc.position, StartRadius);        
        Gizmos.DrawWireSphere(EndLoc.position, EndRadius);
    }
}
