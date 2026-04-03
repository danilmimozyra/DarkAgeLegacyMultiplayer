using System;

public abstract class Command
{
    public abstract String execute(MapState mapS, Player player);
    public abstract boolean exit();
}
