using UnityEngine;
using System.Collections;
using System.Linq;

public class Status : MonoBehaviour
{

    public static Status instance;

    public void Start()
    {
        instance = this;
    }

    public void StartUpdates()
    {
        StartCoroutine(StatusUpdater());
    }

    IEnumerator StatusUpdater()
    {
        yield return new WaitForSeconds(2);
        LowerPanel.instance.Initialize();
        while (true)
        {
            PhotonPlayer[] a = PhotonNetwork.playerList;
            foreach (var photonPlayer in a)
            {
                Debug.Log(photonPlayer.NickName);
            }
            foreach (var person in DataStorage.People)
            {
                bool broken = false;
                foreach (var photonPlayer in a)
                {
                    if (photonPlayer.NickName == person.ID)
                    {
                        person.OnlineStatus = true;
                        broken = true;
                        break;
                    }
                }

                if (!broken)
                {
                    person.OnlineStatus = false;
                }
            }
            LowerPanel.instance.UpdateStatus();
            yield return new WaitForSeconds(1);
        }
    }
}
