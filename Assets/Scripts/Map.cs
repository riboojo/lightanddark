using System.Collections.Generic;

[System.Serializable]
public class Map
{
    public List<Coordinate> units;
    public List<Coordinate> empty;
    public List<Coordinate> safes;
    public List<Coordinate> blockers;
    public Coordinate portalin;
    public Coordinate portalout;
    public Coordinate begin;
    public Coordinate end;
    public int tutorial;
}
