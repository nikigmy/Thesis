﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataStorage
{
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

    public static List<Person> People;

    public static List<Game> Games;
}