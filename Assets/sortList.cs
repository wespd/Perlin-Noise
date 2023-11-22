using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class sortList : MonoBehaviour
{
    public List<Vector2> vectors = new();
   
    public Vector2 compareVector;


    void OnValidate()
    {
        List<Vector2> sortedList = vectors.OrderBy(v => Vector2.Distance(v, compareVector)).ToList();
        foreach (Vector2 sortedVector in sortedList)
        {
            Debug.Log(sortedVector);
        }
    } 
}
