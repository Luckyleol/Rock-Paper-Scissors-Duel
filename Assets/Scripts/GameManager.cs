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
    GameObject timerBar;
    [SerializeField]
    bool playAgainstBot;
    
    int player1Score, player2Score;
    float timer;
    int roundCounter=0;
    float startTimer;
    Player player1, player2;
    Dictionary<Player, bool> hasAttacked;
    Dictionary<Player, RPS> playerInput;

    bool gameEnded;
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
        DuelSuccess success = DuelSuccess.draw;

        //print(timer);
        if (roundCounter <= 15&&!gameEnded)
        {
            if (timer <= 0 && timer > -1)
            {
                if (playAgainstBot)
                {
                    playerInput[player2] = (RPS)Random.Range(0, 2);
                }
                success = CompareInputs();
                print("round " + (roundCounter + 1) + ": " + success);
                timer = GetRoundTime();
                roundCounter++;
                playerInput[player1] = RPS.fail;
                playerInput[player2] = RPS.fail;

                hasAttacked[player1] = false;
                hasAttacked[player2] = false;
            }
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.text = timer.ToString("0.00");
                timerBar.transform.localScale = new Vector3(timer / GetRoundTime(), 1, 1);
            }

            if (success == DuelSuccess.P1)
            {
                transform.Translate(Vector2.right);
                if (!CheckIfOverArena(player2.transform.position)) 
                {
                    player2.Lost();
                    gameEnded = true;
                }

            }
            else
            if (success == DuelSuccess.P2)
            {
                transform.Translate(Vector2.left);
                if (!CheckIfOverArena(player1.transform.position))
                {
                    player1.Lost();
                    gameEnded = true;
                }
            }
            else
            {
                //stay put
            }
            

        }
        
       
    }
    bool CheckIfOverArena(Vector2 position)
    {
        return Physics2D.Raycast(position, Vector2.down, 5);
        
    }

    DuelSuccess CompareInputs()
    {
        if (playerInput[player1] == RPS.fail)
        {
            if (playerInput[player2] == RPS.fail)
            {
                return DuelSuccess.draw;
            }
            return DuelSuccess.P2;
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
        
        if (roundCounter > 15)
        {
            return -1;
        }  
        switch (roundCounter)
        {
            case 0:
                return 1.5f; 
            case 1:
                return 1.25f;
            case 2:
                return 1f;
            case 3:
                return .75f;
            default:
                return .5f;
        }
       

    }



    
}
