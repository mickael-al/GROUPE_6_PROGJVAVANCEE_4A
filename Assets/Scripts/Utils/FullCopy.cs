using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Utils
{
    public static T FullCopy<T>(this T obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Position = 0;
            return (T) bf.Deserialize(ms);
        }
    }
}
