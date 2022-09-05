using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[System.Serializable]
public class WallsArea
{
    public int xLeft;
    public int xRight;
    public int yBottom;
    public int yTop;
}

[System.Serializable]
public class FoodsArea
{
    public int xLeft;
    public int xRight;
    public int yBottom;
    public int yTop;
}

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid I;

    //Map position
    public int xLeft;
    public int xRight;
    public int yBottom;
    public int yTop;

    //Coordinate
    public GameObject[,] GameGrid;
    private int _gridSize;
    [SerializeField]private WallsArea[] wallsAreaArray;
    [SerializeField]private FoodsArea[] foodsAreaArray;
    
    private bool _initialStart;

    public void IncreaseGridSize()
    {
        _gridSize++;
    }
    public void DecreaseGridSize()
    {
        _gridSize--;
    }

    public void EatFood(Vector3 theEatingPosition)
    {
        Destroy(GameGrid[(int) theEatingPosition.x - xLeft, (int) theEatingPosition.y - yBottom]);
        GameGrid[(int) theEatingPosition.x - xLeft, (int) theEatingPosition.y - yBottom] = null;
        
    }

    public string CheckObstacle(Vector3 thePosition)
    {
        if (GameGrid[(int) thePosition.x - xLeft, (int) thePosition.y - yBottom] == null)
        {
            return "Null";
        }
        return GameGrid[(int) thePosition.x - xLeft, (int) thePosition.y - yBottom].tag;
    }

    private void NullifyGrid()
    {
        for (int i = 0; i <= xRight - xLeft; i++)
        {
            for (int j = 0; j <= yTop - yBottom; j++)
            {
                GameGrid[i , j] = null;
            }
        }
    }
    
    private void SpawnInteractable(){
        
        //Spawn Wall Prefabs
        foreach (var wall in wallsAreaArray)
        {
            for (int j = 0; j <= wall.xRight - wall.xLeft; j++)
            {
                for (int k = 0; k <= wall.yTop-wall.yBottom; k++)
                {
                    Vector3 checkPosition = new Vector3(j + wall.xLeft, k + wall.yBottom);
                    if(CheckObstacle(checkPosition) != "Null") continue;
                    GameObject wallGo = Instantiate(GameAssets.I.WallSprite(), checkPosition, new Quaternion());
                    GameGrid[j+wall.xLeft-xLeft,k+wall.yBottom-yBottom] = wallGo;
                    DecreaseGridSize();
                }
            }
        }

        //Spawn Food Prefabs
        foreach (var food in foodsAreaArray)
        {
            for (int j = 0; j <= food.xRight - food.xLeft; j++)
            {
                for (int k = 0; k <= food.yTop-food.yBottom; k++)
                {
                    Vector3 checkPosition = new Vector3(j + food.xLeft, k + food.yBottom);
                    if(CheckObstacle(checkPosition) != "Null") continue;
                    
                    GameObject foodGO = Instantiate(GameAssets.I.RandomFoodSprite(), checkPosition, new Quaternion());
                    GameGrid[j+food.xLeft-xLeft,k+food.yBottom-yBottom] = foodGO;
                    DecreaseGridSize();
                }
            }
        }
    }
    
    void Start()
    {
        I = this;
        _initialStart = true;
        
        GameGrid = new GameObject[xRight-xLeft+1,yTop-yBottom+1];
        _gridSize = (xRight - xLeft + 1) * (yTop - yBottom + 1);
    }
    
    void Update()
    {
        if (!_initialStart) return;
        
        NullifyGrid();
        SpawnInteractable();
        
        _initialStart = false;
    }
    

}
