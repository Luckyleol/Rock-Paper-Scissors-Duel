using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPS
{
    rock,paper,scissors,fail
}

public class GameManager : MonoBehaviour
{
    enum DuelSuccess{
        P1, P2,draw
    }
    
    int player1Score, player2Score;
    float timer;
    int roundCounter=-1;
    float startTimer;
    Player player1, player2;
    Dictionary<Player, RPS> playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Dictionary<Player, RPS>();
        player1 = transform.GetChild(0).GetComponent<Player>();
        player2 = transform.GetChild(1).GetComponent<Player>();
        playerInput.Add(player1, RPS.fail);
        playerInput.Add(player2, RPS.fail);
        timer = 2;
    }

    // Update is called once per frame
    void Update()
    {

        
        //print(timer);
        if (timer <=0)
        {
            print(CompareInputs());
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }

    DuelSuccess CompareInputs()
    {
        if (playerInput[player1] == playerInput[player2])
        {
            return DuelSuccess.draw;
        }else
        if (playerInput[player1]==RPS.rock)
        {
            if (playerInput[player2] == RPS.paper)
            {
                return DuelSuccess.P2;
            }else 
            {
                return DuelSuccess.P1;
            }
        }else if (playerInput[player1] == RPS.paper)
        {
            if (playerInput[player2] == RPS.scissors)
            {
                return DuelSuccess.P2;
            }
            else
            {
                return DuelSuccess.P1;
            }
        }
        else if (playerInput[player1] == RPS.scissors)
        {
            if (playerInput[player2] == RPS.rock)
            {
                return DuelSuccess.P2;
            }
            else
            {
                return DuelSuccess.P1;
            }
        }
        return DuelSuccess.draw;
    }
    public void PlayerAttack(Player player,RPS input)
    {
        //if (timer > 0.1f)
        //{
        //    playerInput[player] = RPS.fail;
        //    return;
        //}
        playerInput[player] = input;
    }
    float GetRoundTime()
    {
        if (roundCounter > 10)
        {
            return -1;
        }
        switch (roundCounter)
        {
            case 0:
                return 2; 
            case 1:
                return 1.75f;
            case 2:
                return 1.5f;
            case 3:
                return 1.25f;
            case 4:
                return 1f;
            case 5:
                return .75f;
            case 6:
                return .5f;
            default:
                return .25f;
        }

    }



    
}
