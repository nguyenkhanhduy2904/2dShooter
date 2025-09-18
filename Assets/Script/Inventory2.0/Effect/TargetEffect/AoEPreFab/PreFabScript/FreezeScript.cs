using UnityEngine;
using System.Collections.Generic;

public class FreezeScript : MonoBehaviour
{
    public CircleCollider2D myCollider;
    public ContactFilter2D filter;
    public float duration = 4f;

    void Start()
    {
        //List<Collider2D> results = new List<Collider2D> ();
       
        //int count = myCollider.Overlap(filter, results);

        //for (int i = 0; i < count; i++)
        //{
        //    var _enemy = results[i].GetComponent<EnemyBehaviour>();
        //    if (_enemy != null)
        //    {
        //        Debug.Log(_enemy.name + " froze");
        //        _enemy.ForceStopAllAction(duration);
        //    }
        //}
    }

}
