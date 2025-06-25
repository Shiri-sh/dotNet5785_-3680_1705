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
    public BlAlreadyExistsException(string message, Exception innerException): base(message, innerException) { }

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
//Request denied due to permission issues
public class BlNotAloudToDoException : Exception
{
    public BlNotAloudToDoException(string? message) : base(message) { }

}
//Incorrect information type entered.
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }

}
public class BLTemporaryNotAvailableException : Exception
{
    public BLTemporaryNotAvailableException(string? message) : base(message) { }

}


