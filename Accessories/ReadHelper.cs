

namespace Accessories;

public class ReadHelper
{
    public static int ReadInt(/*int? minValue = null, int? maxValue = null*/)
    {
   
        string numInput = Console.ReadLine();
        int number;
        while (!int.TryParse(numInput, out number)
           /* &&
            minValue != null ? number >= minValue : true
            &&
            maxValue != null ? number <= minValue : true*/
         )
        {
            Console.WriteLine("invalid input");
            numInput = Console.ReadLine();
        }
        return number;

    }
    public static double ReadDouble()
    {

        string ageInput = Console.ReadLine();
        double data;
        while (!double.TryParse(ageInput, out data))
        {
            Console.WriteLine("invalid input");
            ageInput = Console.ReadLine();
        }
        return data;

    }

    public static DateTime ReadDate()
    {
        
        string dateInput = Console.ReadLine();
        DateTime data;
        while (!DateTime.TryParse(dateInput, out data))
        {
            Console.WriteLine("invalid input");
            dateInput = Console.ReadLine();
        }
        return data;

    }

    public static T ReadEnum<T>()
    {
        string? dateInput = Console.ReadLine();
        object data;
        while (!Enum.TryParse(typeof(T), dateInput, out data))
        {
            Console.WriteLine("invalid input");
            dateInput = Console.ReadLine();
        }
        return (T)data;

    }
    public static T? ReadEnumOrNull<T>() where T : struct, Enum
    {
        string? dateInput = Console.ReadLine();
        if (Enum.TryParse<T>(dateInput, out T result))
        {
            return result;
        }
        return null;
    }
    public static string ReadString()
    {
        string input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Input cannot be empty. Please try again!");
            input = Console.ReadLine();
        }
        return input;
    }
}