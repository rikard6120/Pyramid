// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using System.Xml.XPath;
using Glasses;

Console.WriteLine("Ange antal rader i pyramid:");
var rowInput = Console.ReadLine();
Console.WriteLine("Ange en position för glas:");
var positionInput = Console.ReadLine();

//försöker konvertera variabler till positiva heltal.
UInt32.TryParse(rowInput, out var row);
UInt32.TryParse(positionInput, out var position);

//skapar ett glas på rätt position och skriver ut tiden av intresse.
var glass = new Glass(row, position);
Console.WriteLine(
    "Det tar "
        + glass.GetTime().ToString()
        + " sekunder att fylla glaset på rad "
        + row.ToString()
        + " och position "
        + position.ToString()
        + "."
);
