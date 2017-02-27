using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LowerPanel : MonoBehaviour
{
    public GameObject FriendContent;
    public List<Friend> Friends;

    public static LowerPanel instance;

    void Awake()
    {
        instance = this;
    }

    public void UpdateStatus()
    {
        DataStorage.Person[] friends = new DataStorage.Person[DataStorage.People.Count];
        for (int i = 0; i < DataStorage.People.Count; i++)
        {
            friends[i] = DataStorage.People[i];
        }

        //friends = friends.OrderBy(x => x.OnlineStatus).ToArray();
        for (int i = 0; i < friends.Length; i++)
        {
            Friends[i].FillData(friends[i]);
        }
    }
}
