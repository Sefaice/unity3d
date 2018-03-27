using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class indexUI : MonoBehaviour {

    public Texture imgPlayer1;
    public Texture imgPlayer2;
    private int[,] buttons = new int[3, 3];//buttons check they are clicked by who
    private int choice = 0;//1 for pvp, 2 for pve
    private int turn = 1;//1 for player, 2 for ai 
    private string buttoninfo = "restart";
    private string turninfo = "Player1's turn";
    private int aix = -1;
    private int aiy = -1;

    void start()
    {
        initialize();
    }

    void initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttons[i, j] = 0;
            }
        }
        choice = 0;
    }

    void OnGUI()
    {
        if (choice == 0)
        {
            if (GUI.Button(new Rect((Screen.width - 280) / 2 + 65, (Screen.height - 280) / 2+60, 150, 50), "Player vs Player"))
            {
                choice = 1;
                turn = 1;
            }
            if (GUI.Button(new Rect((Screen.width - 280) / 2 + 65, (Screen.height - 280) / 2+160, 150, 50), "Player vs Computer"))
            {
                choice = 2;
                //random turn
                System.Random ran = new System.Random();
                turn = ran.Next(1,3);
                Debug.Log(turn);
            }

        }
        if (choice == 1)//pvp
        {
            checkEnd();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //Debug.Log(buttons[i, j]);
                    if (buttons[i, j] == 1)
                    {
                        GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), imgPlayer1);
                    }
                    if (buttons[i, j] == 2)
                    {
                        GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), imgPlayer2);
                    }
                    if (GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), ""))//初始时9个按钮从这出现
                    {
                        if (turn == 1)
                        {
                            buttons[i, j] = 1;
                            turn = 2;
                            turninfo = "Player2's turn";
                            // Debug.Log(i);
                            //Debug.Log(j);
                            //Debug.Log(buttons[i,j]);
                        }
                        else
                        {
                            buttons[i, j] = 2;
                            turn = 1;
                            turninfo = "Player1's turn";
                        }
                    }
                }
            }

            GUI.Box(new Rect((Screen.width - 280) / 2 + 300, (Screen.height - 280) / 2 + 0, 100, 25), turninfo);

            if (GUI.Button(new Rect((Screen.width - 280) / 2 + 65, (Screen.height - 280) / 2 + 300, 150, 50), buttoninfo))
            {
                buttoninfo = "restart";
                initialize();
            }
        }
        else if (choice == 2)//pve
        {
            checkEnd();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (buttons[i, j] == 1)
                    {
                        GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), imgPlayer1);
                    }
                    if (buttons[i, j] == 2)
                    {
                        GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), imgPlayer2);
                    }
                    if (GUI.Button(new Rect((Screen.width - 280) / 2 + i * 100, (Screen.height - 280) / 2 + j * 100, 80, 80), ""))
                    {
                        if (turn == 1)
                        {
                            buttons[i, j] = 1;
                            turn = 2;
                            turninfo = "Player2's turn";                         
                        }                
                    }
                }
            }
            if (turn == 2)//cannot trigger by click, auto-trigger
            {
                AImove();
                buttons[aix, aiy] = 2;
                turn = 1;
                turninfo = "Player1's turn";            
            }
            GUI.Box(new Rect((Screen.width - 280) / 2 + 300, (Screen.height - 280) / 2 + 0, 100, 25), turninfo);

            if (GUI.Button(new Rect((Screen.width - 280) / 2 + 65, (Screen.height - 280) / 2 + 300, 150, 50), buttoninfo))
            {
                buttoninfo = "restart";
                initialize();
            }
        }

    }

    void AImove()
    {
        //if win
        for (int q = 0; q < 3; q++)//row
        {
            if (buttons[q, 0] == 2 && buttons[q, 1] == 2 && buttons[q, 2] == 0) { aix = q; aiy = 2; return; }
            if (buttons[q, 0] == 2 && buttons[q, 1] == 0 && buttons[q, 2] == 2) { aix = q; aiy = 1; return; }
            if (buttons[q, 0] == 0 && buttons[q, 1] == 2 && buttons[q, 2] == 2) { aix = q; aiy = 0; return; }
        }
        for (int w = 0; w < 3; w++)//column
        {
            if (buttons[0, w] == 2 && buttons[1, w] == 2 && buttons[2, w] == 0) { aix = 2; aiy = w; return; }
            if (buttons[0, w] == 2 && buttons[1, w] == 0 && buttons[2, w] == 2) { aix = 1; aiy = w; return; }
            if (buttons[0, w] == 0 && buttons[1, w] == 2 && buttons[2, w] == 2) { aix = 0; aiy = w; return; }
        }
        if (buttons[1, 1] == 2 && buttons[2, 2] == 2 && buttons[0, 0] == 0) { aix = 0; aiy = 0; return; }
        if (buttons[1, 1] == 2 && buttons[2, 2] == 0 && buttons[0, 0] == 2) { aix = 2; aiy = 2; return; }
        if (buttons[1, 1] == 0 && buttons[2, 2] == 2 && buttons[0, 0] == 2) { aix = 1; aiy = 1; return; }

        if (buttons[0, 2] == 2 && buttons[1, 1] == 2 && buttons[2, 0] == 0) { aix = 2; aiy = 0; return; }
        if (buttons[0, 2] == 2 && buttons[1, 1] == 0 && buttons[2, 0] == 2) { aix = 1; aiy = 1; return; }
        if (buttons[0, 2] == 0 && buttons[1, 1] == 2 && buttons[2, 0] == 2) { aix = 0; aiy = 2; return; }

        //if die block player
        for (int q = 0; q < 3; q++)//row
        {
            if (buttons[q, 0] == 1 && buttons[q, 1] == 1 && buttons[q, 2] == 0) { aix = q; aiy = 2; return; }
            if (buttons[q, 0] == 1 && buttons[q, 1] == 0 && buttons[q, 2] == 1) { aix = q; aiy = 1; return; }
            if (buttons[q, 0] == 0 && buttons[q, 1] == 1 && buttons[q, 2] == 1) { aix = q; aiy = 0; return; }
        }
        for (int w = 0; w < 3; w++)//column
        {
            if (buttons[0, w] == 1 && buttons[1, w] == 1 && buttons[2, w] == 0) { aix = 2; aiy = w; return; }
            if (buttons[0, w] == 1 && buttons[1, w] == 0 && buttons[2, w] == 1) { aix = 1; aiy = w; return; }
            if (buttons[0, w] == 0 && buttons[1, w] == 1 && buttons[2, w] == 1) { aix = 0; aiy = w; return; }
        }
        if (buttons[1, 1] == 1 && buttons[2, 2] == 1 && buttons[0, 0] == 0) { aix = 0; aiy = 0; return; }
        if (buttons[1, 1] == 1 && buttons[2, 2] == 0 && buttons[0, 0] == 1) { aix = 2; aiy = 2; return; }
        if (buttons[1, 1] == 0 && buttons[2, 2] == 1 && buttons[0, 0] == 1) { aix = 1; aiy = 1; return; }

        if (buttons[0, 2] == 1 && buttons[1, 1] == 1 && buttons[2, 0] == 0) { aix = 2; aiy = 0; return; }
        if (buttons[0, 2] == 1 && buttons[1, 1] == 0 && buttons[2, 0] == 1) { aix = 1; aiy = 1; return; }
        if (buttons[0, 2] == 0 && buttons[1, 1] == 1 && buttons[2, 0] == 1) { aix = 0; aiy = 2; return; }

        //random
        System.Random ran = new System.Random();
        while (true)
        {
            aix = ran.Next(0,3);//0-2 not include maxvalue
            aiy = ran.Next(0,3);
            if (buttons[aix, aiy] == 0) return;
            if (ifDraw() == true) return;
        }
    }

    void checkEnd()//check if end
    {
        //row
        for(int i = 0; i < 3; i++)
        {
            if(buttons[i,0]==buttons[i,1]&& buttons[i, 1] == buttons[i, 2])
            {
                if (buttons[i, 0] == 1)
                {
                    buttoninfo = "Player1 Win!";
                    return;
                }
                if(buttons[i, 0] == 2)
                {
                    buttoninfo = "Player2 Win!";
                    return;
                }
            }
        }
        //column
        for(int j = 0; j < 3; j++)
        {
            if (buttons[0,j] == buttons[1,j] && buttons[1,j] == buttons[2,j])
            {
                if (buttons[0,j] == 1)
                {
                    buttoninfo = "Player1 Win!";
                    return;
                }
                if(buttons[0, j] == 2)
                {
                    buttoninfo = "Player2 Win!";
                    return;
                }
            }
        }
        //xiezhed
        if (buttons[1, 1] == buttons[2, 2] && buttons[0, 0] == buttons[2, 2])
        {
            if (buttons[1, 1] == 1)
            {
                buttoninfo = "Player1 Win!";
                return;
            }
            if(buttons[1, 1] == 2)
            {
                buttoninfo = "Player2 Win!";
                return;
            }
        }
        if (buttons[2, 0] == buttons[1, 1] && buttons[1, 1] == buttons[0, 2])
        {
            if (buttons[1, 1] == 1)
            {
                buttoninfo = "Player1 Win!";
                return;
            }
            if (buttons[1, 1] == 2)
            {
                buttoninfo = "Player2 Win!";
                return;
            }
        }
        //draw
        if (ifDraw() == true)
            buttoninfo = "Draw";
    }

    bool ifDraw()
    {
        int d = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (buttons[i, j] == 0)
                    d = 0;
            }
        }
        if (d == 1) return true;
        else return false;
    }
}




