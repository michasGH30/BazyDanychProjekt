namespace bazyProjektBlazor.Utilities
{
    public class StringOperations
    {
        public static string Capitalise(string text)
        {

            return $"{text[0].ToString().ToUpper()}{text[1..]}";
        }
    }
}
