using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controll 
{
     public static int horizontal_move;
    public static bool up_move;
    public static bool start_up_move;
    public static int jumpLong;
    public static bool exit;
    public static bool down_move;

    public static float GetHorizontalMove()
    {
        if (Input.GetAxis("Horizontal") != 0) return Input.GetAxis("Horizontal");
        else return horizontal_move;
    }
    public static bool GetJumpStart()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return Input.GetKeyDown(KeyCode.Space);
        return start_up_move;
    }
    public static bool GetJumpHold()
    {
        if ( Input.GetKey(KeyCode.Space)) return Input.GetKey(KeyCode.Space);
        return up_move;
    }
    public static bool GetJumpStop()
    {
        if (!Input.GetKeyUp(KeyCode.Space)) return Input.GetKeyUp(KeyCode.Space);
        return !up_move;
    }
    public static bool GetDown()
    {
        if (Input.GetKey(KeyCode.S)) return Input.GetKey(KeyCode.S);
        return down_move;
    }
    public static bool GetExit()
    {
        if (Input.GetKey(KeyCode.Q)) return Input.GetKey(KeyCode.Q);
        return exit;
    }



    public static void Start()
    {
        KinectManager.Left += new KinectManager.SimpleEvent(Left);
        KinectManager.Right += new KinectManager.SimpleEvent(Right);
        KinectManager.Up += new KinectManager.SimpleEvent(Up);
        KinectManager.Down += new KinectManager.SimpleEvent(Down);
        KinectManager.Stop += new KinectManager.SimpleEvent(Stop);
        KinectManager.StopUp += new KinectManager.SimpleEvent(StopUp);
        KinectManager.StopDown += new KinectManager.SimpleEvent(StopDown);
        KinectManager.Exit += new KinectManager.SimpleEvent(Exit);
        KinectManager.StopExit += new KinectManager.SimpleEvent(StopExit);
    }

    static void Left()
    {
        if (horizontal_move == 0) horizontal_move = -1;
        Debug.Log(horizontal_move);
    }
    static void Right()
    {
        if (horizontal_move == 0) horizontal_move = 1;
    }
    static void Down()
    {
        down_move = true;
    }

    static void Up()
    {
        if (up_move) start_up_move = false;
        else start_up_move = true;
        up_move = true;
    }

    static void StopDown()
    {
        down_move = false;
    }

    static void StopUp()
    {
        up_move = false;
        start_up_move = false;
    }

    static void Stop()
    {
        horizontal_move = 0;
    }
    static void Exit()
    {
        exit = true;
    }
    static void StopExit()
    {
        exit = false;
    }


}
