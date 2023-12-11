public class SetupNode : ActiveNode
{
    public SetupNode(bool force, string parentName, string name, RunnableMonoBehaviour runnableMonoBehaviour) : base(force, parentName, name, runnableMonoBehaviour)
    {

    }

    public ActiveNode Force() //alert zatrzymanie poprzednich a dokładne wykonanie to powinny być 2 różne rzeczy
    {
        GetRunnableMonoBehaviour().Stop(GetParentName());
        
        return new ActiveNode(true, GetParentName(), GetName(), GetRunnableMonoBehaviour());
    }
}
