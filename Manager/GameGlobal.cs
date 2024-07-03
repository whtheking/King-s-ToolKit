using System.Diagnostics;

public class GameGlobal : Manager<GameGlobal>
{
    
    
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        
    }

    public override void Tick(float dt)
    {
        throw new System.NotImplementedException();
    }

    public override void Uninit()
    {
        throw new System.NotImplementedException();
    }
}