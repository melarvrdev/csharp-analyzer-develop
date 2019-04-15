using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "One for you, one for me.";

        return "One for " + input + ", one for me.";
    }
}