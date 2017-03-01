using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DataStorage
{
    public class GameInfo
    {
        public string Name;
        public string Description;
        public Sprite Image;
        public int Index;
        public bool Multiplayer;
        public bool HasHighscores;

        public GameInfo(string name, string description, Sprite image, int index, bool multiplayer, bool hasHighscores)
        {
            Name = name;
            Description = description;
            Image = image;
            Index = index;
            Multiplayer = multiplayer;
            HasHighscores = hasHighscores;
        }
    }
    public class Person
    {
        public Person()
        {

        }

        public Person(string id, string name, Sprite profilePicture)
        {
            ID = id;
            Name = name;
            ProfilePicture = profilePicture;
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public Sprite ProfilePicture { get; set; }

        public bool OnlineStatus { get; set; }
    }

    public class Game
    {
        public Game()
        {

        }

        public Game(string name, Sprite picture, string description)
        {
            Name = name;
            Picture = picture;
            Description = description;
        }

        public string Name { get; set; }

        public Sprite Picture { get; set; }

        public string Description { get; set; }
    }

    public static List<string> OnlineFriends;

    public static Person ThisUser;

    public static List<Person> People;

    public static List<Game> Games;

    public static void UpdateStatus()
    {
        foreach (var person in People)
        {
            if (OnlineFriends.Contains(person.ID))
            {
                person.OnlineStatus = true;
            }
            else
            {
                person.OnlineStatus = false;
            }
        }

        People = People.OrderByDescending(x => x.OnlineStatus).ToList();
    }
}