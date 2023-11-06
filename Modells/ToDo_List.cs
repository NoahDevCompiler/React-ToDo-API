using System;

public class ToDo_Modell
{
    public int Id { get; set; }


    public string Name { get; set; }


    public string Description { get; set; }


    public string Type { get; set; }


    public DateTime Startdate { get; set; }


    public DateTime Enddate { get; set; }


    public ToDo_Modell(int id, string name, string description, string type, DateTime startdate, DateTime enddate)
	{
        Id = id;

        Name = name;

        Description = description;

        Type = type;

        Startdate = startdate;

        Enddate = enddate;
		
	}
}
