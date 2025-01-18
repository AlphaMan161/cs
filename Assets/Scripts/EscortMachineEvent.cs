// ILSpyBased#2
public class EscortMachineEvent
{
    public EscortMachineEventType EscortMachineEventType;

    public EscortMachine EscortMachine;

    public float Progress;

    public CombatPlayer ActivePlayer;

    public EscortMachineEvent(EscortMachineEventType EscortMachineEventType, EscortMachine EscortMachine, float Progress)
    {
        this.EscortMachineEventType = EscortMachineEventType;
        this.EscortMachine = EscortMachine;
        this.Progress = Progress;
    }

    public EscortMachineEvent(EscortMachineEventType EscortMachineEventType, EscortMachine EscortMachine)
    {
        this.EscortMachineEventType = EscortMachineEventType;
        this.EscortMachine = EscortMachine;
    }

    public EscortMachineEvent(EscortMachineEventType EscortMachineEventType, EscortMachine EscortMachine, CombatPlayer ActivePlayer)
    {
        this.EscortMachineEventType = EscortMachineEventType;
        this.EscortMachine = EscortMachine;
        this.ActivePlayer = ActivePlayer;
    }
}


