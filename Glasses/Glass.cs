namespace Glasses;

public class Glass
{
    private uint Row; //Raden glaset står på
    private uint Position; //Glasets position
    private double VolumeOfliquid = 0; //Volymen vätska i glaset
    private Glass? LeftGlassAbove { get; set; } = null; //Information om eventuellt glas ovan till vänster
    private Glass? RightGlassAbove { get; set; } = null; //information om eventuellt glas ovan till höger

    public Glass(uint row, uint position)
    {
        if (row != 0)
        {
            Row = row;
        }
        else
        {
            throw new ArgumentException("Antalet rader måste vara ett heltal större än noll");
        }
        if (position >= 1 && position <= row)
        {
            Position = position;
        }
        else
        {
            throw new ArgumentException(
                "Positionen måste vara ett heltal större än noll och mindre eller lika med antalet rader."
            );
        }
    }

    public double GetTime()
    {
        /*Metod för att räkna ut tiden att fylla glas. Eftersom att flödet av vätska in i glaset
        mäts i volymenheter per sekund kommer antalet sekunder det tar att fylla ett glas vara lika med
        antalet iterationer nedan, minus ett eventuellt avdrag om glaset blir överfyllt i den sista iterationen.*/
        Structure();
        double timeToFill = 0;
        while (VolumeOfliquid < 10)
        {
            VolumeOfliquid += InFlow();
            timeToFill += 1;
            if (VolumeOfliquid > 10)
            {
                double dVolume = VolumeOfliquid - 10;
                double dTime = dVolume / InFlow();
                timeToFill -= dTime;
            }
        }
        return Math.Round(timeToFill, 3);
    }

    private double InFlow()
    {
        //Metod som returnerar det totala inflödet i glaset som funktion av utflödet från glasen ovan
        double inFlow;
        if (LeftGlassAbove is not null && RightGlassAbove is not null)
        {
            inFlow = LeftGlassAbove.OutFlow() + RightGlassAbove.OutFlow();
        }
        else if (LeftGlassAbove is null && RightGlassAbove is not null)
        {
            inFlow = RightGlassAbove.OutFlow();
        }
        else if (LeftGlassAbove is not null && RightGlassAbove is null)
        {
            inFlow = LeftGlassAbove.OutFlow();
        }
        else
        {
            /*Fallet för det översta glaset. Enheter för inflöde och volym är valda så att
            det tar 10 sekunder att fylla detta glas*/
            inFlow = 1;
        }
        return inFlow;
    }

    private double OutFlow()
    {
        /*Metod som returnerar det totala inflödet i glaset som funktion av utflödet från glasen.
        utflödet är noll om glaset inte är fyllt (när volymen av vätska = 10).*/
        double outFlow;
        if (VolumeOfliquid > 10)
        {
            outFlow = (VolumeOfliquid - 10 + InFlow()) / 2;
            VolumeOfliquid = 10;
        }
        else if (VolumeOfliquid == 10)
        {
            outFlow = InFlow() / 2;
        }
        else
        {
            VolumeOfliquid += InFlow();
            outFlow = 0;
        }
        return outFlow;
    }

    private void Structure()
    {
        /*Metod som skapar en binär träd-liknande struktur som innehåller informationen
        om glas ovan glaset av intresse, samt om glas ovanför de glasen etc. */
        if (Row <= 1)
        {
            return;
        }

        if (Position - 1 > 0)
        {
            LeftGlassAbove = new Glass(Row - 1, Position - 1);
        }

        if (Position <= Row - 1)
        {
            RightGlassAbove = new Glass(Row - 1, Position);
        }
        LeftGlassAbove?.Structure();
        RightGlassAbove?.Structure();
    }
}
