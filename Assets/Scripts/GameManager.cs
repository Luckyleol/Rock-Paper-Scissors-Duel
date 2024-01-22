using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    TextMeshProUGUI timerText,victoryText,roundCounterText,p1Action,p2Action;
    [SerializeField]
    GameObject StartGameUI, EndGameUI,DuelText;
    [SerializeField]
    GameObject timerBar;
    [SerializeField]
    bool playAgainstBot;
    
    int player1Score, player2Score;
    float timer;
    int roundCounter=-1;
    float startTimer;
    Player player1, player2;
    Vector2 p1StartPos, p2StartPos;
    Dictionary<Player, bool> hasAttacked;
    Dictionary<Player, RPS> playerInput;

    AudioSource playerAudio;
    [SerializeField]
    AudioClip hitClip, drawClip;

    bool gameEnded;
    private void Awake()
    {
        playerAudio= GetComponent<AudioSource>();
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
            //player2.enabled = false;
        }
        p1StartPos = player1.transform.localPosition;
        p2StartPos = player2.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
       
        
    }

    // Update is called once per frame
    void Update()
    {
        DuelSuccess success = DuelSuccess.draw;
        if (roundCounter == -1 )
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();
           
            if (timer <= 0)
            {
                roundCounter = 0;
                timer = 1;
                DuelText.SetActive(true);
                StartGameUI.SetActive(false);
                roundCounterText.gameObject.SetActive(true);
               
            }

            return;
        }
        
        
        //print(timer);
        if (roundCounter < 10&&!gameEnded)
        {
            roundCounterText.text = "Round: " + (roundCounter+1);
            //player1.transform.localPosition = p1StartPos;
            //player2.transform.localPosition = p2StartPos;
            if (timer <= 0 && timer > -1)
            {
                if (playAgainstBot)
                {
                    playerInput[player2] = (RPS)Random.Range(0, 2);
                    hasAttacked[player2] = true;
                }
                success = CompareInputs();
                print("round " + (roundCounter + 1) + ": " + success);

                timer = 1;
                roundCounter++;
                p1Action.text = playerInput[player1].ToString().ToUpper();
                p2Action.text = playerInput[player2].ToString().ToUpper();
                p1Action.gameObject.SetActive(true);
                p2Action.gameObject.SetActive(true);
                playerInput[player1] = RPS.fail;
                playerInput[player2] = RPS.fail;

                hasAttacked[player1] = false;
                hasAttacked[player2] = false;

                if (success == DuelSuccess.P1)
                {
                    player1.GetComponent<Animator>().SetTrigger("Attack");
                    //player1.transform.Translate(Vector2.right*1);
                    player2.GetComponent<Animator>().SetTrigger("Block");
                    transform.Translate(Vector2.right * 1.25f);
                    if (!CheckIfOverArena(player2.transform.position))
                    {
                        player2.Lost();
                        EndMatch("Green Wins");
                    }
                    playerAudio.PlayOneShot(hitClip);

                }
                else
            if (success == DuelSuccess.P2)
                {
                    player2.GetComponent<Animator>().SetTrigger("Attack");
                    //player2.transform.Translate(Vector2.left * 1);
                    player1.GetComponent<Animator>().SetTrigger("Block");
                    transform.Translate(Vector2.left * 1.25f);
                    if (!CheckIfOverArena(player1.transform.position))
                    {
                        player1.Lost();
                        EndMatch("Blue Wins");

                    }
                    playerAudio.PlayOneShot(hitClip);
                }
                else if (success == DuelSuccess.draw)
                {
                    player1.GetComponent<Animator>().SetTrigger("Attack");
                    player2.GetComponent<Animator>().SetTrigger("Attack");
                    playerAudio.PlayOneShot(drawClip);

                }

            }
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
               
                timerBar.transform.localScale = new Vector3(timer / 1, 1, 1);
            }

            
            


        }
        else
        {
            if (gameEnded)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                return;
            }
            if (!CheckIfOverArena(player2.transform.position))
            {
                player2.Lost();
                EndMatch("Green Wins");
            }else
            if (!CheckIfOverArena(player1.transform.position))
            {
                player1.Lost();
                EndMatch("Blue Wins");

            }
            else
            {
                EndMatch("DRAW");
            }
           
        }
        
       
    }
    public void PlayAgainstBot(bool x)
    {
        playAgainstBot = x;
    }
    void EndMatch(string victorString)
    {
        //Time.timeScale = 0;
        gameEnded = true;
        EndGameUI.SetActive(true);
        victoryText.text=victorString;
        timer = 2;
    }
    bool CheckIfOverArena(Vector2 position)
    {
        return Physics2D.Raycast(position, Vector2.down, 5);
        
    }

    DuelSuccess CompareInputs()
    {
        if (playerInput[player1] == playerInput[player2])
        {
            return DuelSuccess.draw;
        }
        if (playerInput[player1] == RPS.fail)
        {
            if (playerInput[player2] == RPS.fail)
            {
                return DuelSuccess.draw;
            }
            return DuelSuccess.P2;
        }
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

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
