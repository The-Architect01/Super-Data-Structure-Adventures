using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class boardController : MonoBehaviour
{
    //2 lists, one for players still in the game, one for the losers! :D
    public GameObject Incog;
    public GameObject Debugger;
    public GameObject Dipsy;
    public GameObject Brick;

    public LinkedList<GameObject> contestants = new LinkedList<GameObject>();
    LinkedList<GameObject> loserParty = new LinkedList<GameObject>();
    BoardGraph island = new BoardGraph();
    LinkedListNode<GameObject> frontRunner;
    int roundCnt;
    int elimCnt; // minigames will happen on ever xth turn
    int numPlayers;
    bool eliminationRound = false;
    public TextMeshProUGUI bitCoin;
    public TextMeshProUGUI centralText;
    

    void Start()
    {
        //Get the number of players, then create a new contestent for each one
        numPlayers = 5;
        //If we have a variable player, this will need to be converted to a loop of some kind
        contestants.AddLast(Incog);
        contestants.AddLast(Debugger);
        contestants.AddLast(Dipsy);
        contestants.AddLast(Brick);

        elimCnt = 3;
        roundCnt = 1;
        frontRunner = contestants.First;
        bitCoin.text = "Bits: 0";
    }

    // Update is called once per frame
    void Update()
    {
        centralText.text = "Round "+roundCnt;
        if(roundCnt % elimCnt == 0)
        {
            eliminationRound = true;
            Zombie.MiniGameList.randomMinigame();
        }
         for(LinkedListNode<GameObject> current = frontRunner; current.Next != null; current = current.Next)
         {
            //bitCoin.text = "" + current.Value.getBits();
             //current.Value.turnStart();
             //while(current.Value.isTurn)
             //{
                 //I don't know if there's anything to even do here, just wait for the turn to be over! :P 
             //}    
         }
         
        if (eliminationRound)
        {
            ChallengeTime();
        }

        roundCnt++;
    }

    public void ChallengeTime()
    {
        
    }
}
