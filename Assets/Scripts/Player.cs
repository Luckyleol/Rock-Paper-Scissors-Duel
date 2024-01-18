using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(1,2)]
    int playerNumber;
    PlayerInput input;
    

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentControlScheme("Player "+playerNumber, InputSystem.devices[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnRock()
    {
        transform.GetComponentInParent<GameManager>().PlayerAttack(this, RPS.rock);
        //print("rock"+this.gameObject);
    }
    void OnPaper()
    {
        transform.GetComponentInParent<GameManager>().PlayerAttack(this, RPS.paper);
       //print("paper" + this.gameObject);
    }

    void OnScissors()
    {
        transform.GetComponentInParent<GameManager>().PlayerAttack(this, RPS.scissors);
        //print("scissors" + this.gameObject);
    }
}
