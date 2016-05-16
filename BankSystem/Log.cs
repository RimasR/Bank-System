using System;

public class Log :IComparable<Log>       //ICOMPARABLE
{
    public string id;
    public string debug;
    /*public string[] temp = new string[100];
    public string this [int index]                 //INDEXED PRAMETER, no use though
    {
        get
        {
            return temp[index];
        }
        set
        {
            temp[index] = value;
        }
    }*/
    public DateTime debugTime { get; set; }

    public override string ToString()
    {
        return
            String.Format("ID: {0,-10} Date: {1, 6} Event: {2}", id, debugTime, debug);
    }

    public int CompareTo(Log other) //Sort by debug time descending
    {
        if (debugTime < other.debugTime) return 1;
        if (debugTime > other.debugTime) return -1;
        return 0;
    }
}
