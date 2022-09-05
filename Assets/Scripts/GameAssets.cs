using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameAssets I;
    public GameObject[] foodArray;

    public GameObject RandomFoodSprite()
    {
        var choice = Random.Range(0, foodArray.Length);
        return foodArray[choice];
    }
    public GameObject grass;

    public GameObject wall;
    public GameObject WallSprite()
    {
        return wall;
    }
    void Start()
    {
        I = this;
        
    }
}
