//using UnityEngine;
//using System.Collections.Generic;
//using Facebook.Unity;

//public static class DataLayer{

//    public struct BasicData{
//        string usrename;
//        Sprite ProfilePicture;
//    }
    
//    public static BasicData GetBasicData(string ID, Vector2 profilePictureSize)
//    {
//        BasicData data = new BasicData();
//        FB.API(Util.GetPictureURL(ID, (int)profilePictureSize.x, (int)profilePictureSize.y), HttpMethod.GET, ((IGraphResult result) => FillFriendCallback(result, id, name)));
//        return new BasicData();
//    }

//    void BasicDataCallback()
//    public static List<string> GetFriends(string ID)
//    {
//        return null;
//    }

//}
