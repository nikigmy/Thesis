using UnityEngine;
using System.Collections;
using System.Linq;

public class LowerPanel : MonoBehaviour
{
    public GameObject FriendContent;
    private Friend[] Friends;

    public static LowerPanel instance;

    void Start()
    {
        instance = this;
    }

    public void Initialize()
    {
        Friends = new Friend[FriendContent.transform.childCount];
        for (int i = 0; i < Friends.Length; i++)
        {
            GameObject friend = FriendContent.transform.GetChild(i).gameObject;
            Friends[i] = friend.GetComponent<Friend>();
        }
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
