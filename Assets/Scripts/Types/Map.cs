using System.Collections.Generic;

[System.Serializable]
public class Map
{
    public int sizeX;
    public int sizeY;
    public List<Coordinate> borderL;
    public List<Coordinate> borderR;
    public List<Coordinate> borderU;
    public List<Coordinate> borderD;
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
