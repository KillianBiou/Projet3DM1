using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public GameObject GetGround() { return transform.Find("Ground").gameObject; }
    public GameObject GetExit() { return transform.Find("Exit").gameObject; }
}
