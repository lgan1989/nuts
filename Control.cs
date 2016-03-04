using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

    public MenuRender menu;
    public Canvas canvas;

    static Queue<GameEvent> eventQueue;

    public ControlStatus currentStatus;
    public ControlStatus CurrentStatus{
        get{
            return currentStatus;
        }
        set{
            currentStatus = value;
        }
    }

    public enum ActionType{
        Attack,
        Move
    }

    public enum ControlStatus{
        Idle ,
        PlayerPawnSelected ,
        NonPlayerPawnSelected,
        MapTileSelected ,
        ShowMenu ,
        ChooseAttackTarget ,
        Moving ,
        Attacking,
        Finished
    }

    // Use this for initialization

    public List<Status> statusList;

    void InitDFA(){
        statusList = new List<Status>();
        int len = System.Enum.GetNames(typeof(ControlStatus)).Length;
        for (int i = 0 ; i < len ; i ++){
            statusList.Add( new Status() );
            statusList[i].next = new Hashtable();
        }
        //from idle to next

        LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.PlayerPawn  , EventType.LeftClick) , ControlStatus.PlayerPawnSelected);
        LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.FriendPawn , EventType.LeftClick) , ControlStatus.NonPlayerPawnSelected);
        LinkStatus(ControlStatus.Idle , new GameEvent(EventTargetType.EnemyPawn , EventType.LeftClick) , ControlStatus.NonPlayerPawnSelected);

        LinkStatus(ControlStatus.PlayerPawnSelected, new GameEvent( EventTargetType.MoveTile , EventType.LeftClick) , ControlStatus.Moving);
        LinkStatus(ControlStatus.PlayerPawnSelected, new GameEvent( EventTargetType.Any , EventType.RightClick) , ControlStatus.Idle);

        LinkStatus(ControlStatus.Moving , new GameEvent( EventTargetType.None , EventType.MoveFinished) , ControlStatus.ShowMenu);

        LinkStatus(ControlStatus.ShowMenu , new GameEvent( EventTargetType.MenuItemAttack, EventType.LeftClick) , ControlStatus.ChooseAttackTarget);
        LinkStatus(ControlStatus.ShowMenu , new GameEvent( EventTargetType.MenuItemAttack, EventType.RightClick) , ControlStatus.ChooseAttackTarget);
        LinkStatus(ControlStatus.ChooseAttackTarget , new GameEvent(EventTargetType.EnemyPawn , EventType.LeftClick) , ControlStatus.Attacking);
        LinkStatus(ControlStatus.Attacking , new GameEvent(EventTargetType.None , EventType.AttackFinished) , ControlStatus.Idle);

        LinkStatus(ControlStatus.ChooseAttackTarget , new GameEvent(EventTargetType.Any , EventType.RightClick) , ControlStatus.ShowMenu);

        LinkStatus(ControlStatus.Finished , new GameEvent(EventTargetType.None , EventType.ActionFinished) , ControlStatus.Idle);
    }

    void LinkStatus(ControlStatus from , GameEvent evt, ControlStatus to){
        statusList[ (int) from ].next.Add(evt.GetHashCode() , to);
    }

    ControlStatus NextStatus(GameEvent evt){

        if (statusList[(int)currentStatus].next.ContainsKey(evt.GetHashCode()))
            return (ControlStatus)statusList[(int)currentStatus].next[evt.GetHashCode()];
        return currentStatus;
    }


    void Awake(){
        InitDFA();
    }

    void Start () {
        canvas = menu.GetComponent<Canvas> ();
        canvas.enabled = false;
        eventQueue = new Queue<GameEvent>();
    }

    // Update is called once per frame
    void Update () {
        ProcessEvent();
    }

    public static void PushEvent(GameEvent evt){
        eventQueue.Enqueue(evt);
    }

    public IEnumerator React(Pawn attacker, Pawn defender, float delayInSecs, string type)
    {
        yield return new WaitForSeconds(delayInSecs);
        switch (type){
            case "damaged":
                defender.Damaged();
                break;
            case "parry":
                defender.Parry(attacker);
                break;
        }
    }
    public void ProcessEvent(){

        while (eventQueue.Count > 0){
            GameEvent evt = eventQueue.Dequeue();
            ControlStatus next = NextStatus(evt);
            //check if transition is valid

            switch( next ){
                case ControlStatus.PlayerPawnSelected:
                case ControlStatus.NonPlayerPawnSelected:
                    Logic.SelectPawn(evt.obj.GetComponent<Pawn>());
                    currentStatus = next;
                    break;
                case ControlStatus.Attacking:

                    Pawn attacker = Logic.selectedPawn;
                    Pawn defender = evt.obj.GetComponent<Pawn>();
                    if (attacker.IsValidTarget( defender , ActionType.Attack  )){
                        attacker.Attack(defender);
                        StartCoroutine(React(attacker, defender, 0.65f, "parry"));
                        currentStatus = next;
                    }
					break;
                case ControlStatus.Finished:
                    Logic.selectedPawn.StandBy();
                    break;
                default:
                    currentStatus = next;
                    break;
            }
        }
        if (currentStatus != Control.ControlStatus.ShowMenu){
            canvas.enabled = false;
        }

    }

    public void ShowMenu(Vector3 position){
        position.Set(position.x , position.y , -100);

        Transform panel = canvas.transform.FindChild ("Panel");

        float x = position.x > 0.5f ? position.x - 0.5f : position.x + 0.5f;
        float y = position.y > 0.5f ? position.y - 0.5f : position.y + 0.5f;

        position = new Vector3 (x, y, position.z);
        Vector3 panelPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

        menu.transform.FindChild("Panel").transform.position = panelPosition;
        canvas.enabled = true;
    }

    public void HideMenu(){
        canvas.enabled = false;
    }
}
