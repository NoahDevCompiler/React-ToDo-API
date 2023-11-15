﻿using System;

public class Todo
{
   
    public string Name { get; set; }


    public string Description { get; set; }


    public string Type { get; set; }


    public DateTime Startdate { get; set; }


    public DateTime Enddate { get; set; }


    public Todo( string name, string description, string type, DateTime startdate, DateTime enddate)
	{


        Name = name;

        Description = description;

        Type = type;    

        Startdate = startdate;

        Enddate = enddate;
		
	}
}
