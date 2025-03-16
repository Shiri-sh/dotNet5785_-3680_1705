namespace BO;
[Serializable]
//ניסיון לעדכן אוביקט עם מספר מזהה שלא קיים
public class BlDoesNotExistException:Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
//להשתמש בערך שערכו NULL
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
//מספר מזהה שכבר קיים
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}
//אא למחוק
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
}
//לא ניתן ליצר קובץ XML
public class BlXMLFileLoadCreateException : Exception
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }

}

