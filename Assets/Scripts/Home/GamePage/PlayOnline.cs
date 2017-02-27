using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayOnline : MonoBehaviour
{

    public GameObject FriendContent;
    public GameObject friendPrefab;
    public List<FriendForInvites> friends;

    public void ShowFriends()
    {
        DataStorage.Person[] onlineFriends = DataStorage.People.Where(x => x.OnlineStatus).ToArray();

        if (friends.Count > onlineFriends.Length)
        {
            for (int i = onlineFriends.Length; i < friends.Count; i++)
            {
                Destroy(friends[i].gameObject);
            }
        }
        else if (friends.Count < onlineFriends.Length)
        {
            for (int i = friends.Count; i < onlineFriends.Length; i++)
            {
                GameObject friend = Instantiate(friendPrefab, FriendContent.transform);
                friends.Add(friend.GetComponent<FriendForInvites>());
            }
        }

        for (int i = 0; i < onlineFriends.Length; i++)
        {
            friends[i].FillData(onlineFriends[i]);
        }
    }
}
