using System.Collections.Generic;

public static class ContainerNavigation
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum Panel
    {
        Main,
        Top,
        Bottom,
        Left,
        Right
    }

    public static List<Direction> ALL_DIRECTIONS = new List<Direction>() { Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN};

    public static Dictionary<Panel, Dictionary<Direction, Panel>> NAVIGATION_RULES = new Dictionary<Panel, Dictionary<Direction, Panel>>()
    {
        { Panel.Main, new Dictionary<Direction, Panel> { { Direction.LEFT, Panel.Left }, { Direction.RIGHT, Panel.Right },/* { Direction.UP, Panel.Top },*/ { Direction.DOWN, Panel.Bottom } } },
        { Panel.Top, new Dictionary<Direction, Panel> { { Direction.DOWN, Panel.Main } } },
        { Panel.Bottom, new Dictionary<Direction, Panel> { { Direction.UP, Panel.Main } } },
        { Panel.Left, new Dictionary<Direction, Panel> { { Direction.RIGHT, Panel.Main } } },
        { Panel.Right, new Dictionary<Direction, Panel> { { Direction.LEFT, Panel.Main } } },
    };
}
