using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public enum RPS
{
    rock,paper,scissors,fail
}

public class GameManager : MonoBehaviour
{
    enum DuelSuccess{
        P1, P2,draw
    }
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    bool playAgainstBot;
    int player1Score, player2Score;
    float timer;
    int roundCounter=0;
    float startTimer;
    Player player1, player2;
    Dictionary<Player, bool> hasAttacked;
    Dictionary<Player, RPS> playerInput;

    private void Awake()
    {
        playerInput = new Dictionary<Player, RPS>();
        hasAttacked = new Dictionary<Player, bool>();
        player1 = transform.GetChild(0).GetComponent<Player>();
        player2 = transform.GetChild(1).GetComponent<Player>();
        hasAttacked.Add(player1, false);
        hasAttacked.Add(player2, false);
        playerInput.Add(player1, RPS.fail);
        playerInput.Add(player2, RPS.fail);
        timer = 2;
        if (playAgainstBot)
        {
            player2.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
       
        
    }

    // Update is called once per frame
    void Update()
    {

        
        //print(timer);
        if (timer <=0&& timer>-1)
        {
            if (playAgainstBot)
            {
                playerInput[player2] =(RPS)Random.Range(0, 2);
            }

            print("round "+(roundCounter+1)+": " +CompareInputs());
            timer = GetRoundTime();
            roundCounter++;
            playerInput[player1] = RPS.fail;
            playerInput[player2] = RPS.fail;
           
            hasAttacked[player1] = false;
            hasAttacked[player2] = false;
        }
        else if( timer>0)
        {
            timer -= Time.deltaTime;
        }
        timerText.text = timer.ToString();
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
        if (hasAttacked[player])
        {
            return;
        }
        hasAttacked[player] = true;
        //if (timer > 0.25f )
        //{
        //   playerInput[player] = RPS.fail;
        //   return;
        //}
        playerInput[player] = input;
        print(player + ":"+ input);
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
