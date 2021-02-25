public class TagManager
{
    public static string[] SplitByTags(string targetText)
    {
        return targetText.Split(new char[2] { '<', '>' });
    }
}
