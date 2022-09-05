using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PacmanMovement : MonoBehaviour
{
    private Vector3 _position;
    private Transform _tf;
    
    private bool _isSelected= true;

    private int _horizontal;
    private int _vertical;

    private float _currentTimeTick;
    public float maxTimeTick;

    private State _state;
    private enum State
    {
        Alive,
        Dead
    }
    private void Start()
    {
        
        _currentTimeTick = 0f;
        _horizontal = 1;
        _vertical = 0;

        _tf = transform;
        _position = _tf.position;

        _state = State.Alive;
    }

    private void Warp()
    {
        if (_position.x < LevelGrid.I.xLeft) _position.x = LevelGrid.I.xRight;
        else if (_position.x > LevelGrid.I.xRight) _position.x = LevelGrid.I.xLeft;
        else if (_position.y < LevelGrid.I.yBottom) _position.y = LevelGrid.I.yTop;
        else if (_position.y > LevelGrid.I.yTop) _position.y = LevelGrid.I.yBottom;
    }

    private void Move()
    {
        _currentTimeTick += Time.deltaTime;
        
        if (_currentTimeTick >= maxTimeTick)
        {
            //recycle loop
            _currentTimeTick -= maxTimeTick;
            _isSelected = true;
            
            //new Position
            var rotation = new Vector3(0, 0, GetAngelFromVector(_horizontal, _vertical));
            _position = new Vector3(_position.x+_horizontal, _position.y+_vertical,0);
            Warp();
            
            //check if the new position is a wall / food / body 
            string incomingObstacle = LevelGrid.I.CheckObstacle(_position);

            switch (incomingObstacle) // Pacman reaction to incomingObstacle
            {
                case "Wall":
                    _position = new Vector3(_position.x-_horizontal, _position.y-_vertical,0);
                    _tf.eulerAngles = rotation;
                    Warp();
                    break;
                
                case "Ghost":
                    UnityEngine.Debug.Log("Pacman Die");
                    _state = State.Dead;
                    break;
                case "Food":
                    LevelGrid.I.EatFood(_position);
                    _tf.position = _position;
                    _tf.eulerAngles = rotation;
                    break;

                default: // "Null"
                    _tf.position = _position;
                    _tf.eulerAngles = rotation;
                    break;
            }
        }
    }

    private void TurnChecking()
    {
        //To lock the snake from switching state move Up Down or Left Right 
        if ( _isSelected)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _vertical = 0;
                _horizontal = -1;
                _isSelected= false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _vertical = 0;
                _horizontal = 1;
                _isSelected= false;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            { 
                _vertical = 1;
                _horizontal = 0;
                _isSelected= false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _vertical = -1;
                _horizontal = 0;
                _isSelected= false;
            }
        }
        
    }
    private void Update()
    {
        switch (_state)
        {
            case State.Alive:
                Move();
                TurnChecking();
                break;
            case State.Dead:
                break;
        }
    }

    private float GetAngelFromVector(int x, int y)
    {
        float n = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    
}
