using System.Collections;
using System.Collections.Generic;

public class SceneName
{
    public const string MenuScene = "MenuScene";

    public const string MapScene = "MapScene";

    public static string Level(int level)
    {
        return $"Level{level}";
    }

}
