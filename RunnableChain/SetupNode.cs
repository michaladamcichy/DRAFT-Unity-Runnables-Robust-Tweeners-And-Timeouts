public class SetupNode : ActiveNode
{
    public SetupNode(bool force, string parentName, string name, Runnabler runnableMonoBehaviour) : base(force, parentName, name, runnableMonoBehaviour)
    {

    }

    public ActiveNode Force() //alert zatrzymanie poprzednich a dokładne wykonanie to powinny być 2 różne rzeczy
    {
        GetRunnabler().StopIfExists(GetParentName());
        
        return new ActiveNode(true, GetParentName(), GetName(), GetRunnabler());
    }

    public void Speed(float speed)
    {
        GetRunnabler().SetSpeed(GetParentName(), speed);
    }
}
