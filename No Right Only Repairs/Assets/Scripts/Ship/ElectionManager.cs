using System.Collections.Generic;
using UnityEngine;

public enum ElectionModifier
{
    TooManyCooks,             
    LonelinessEpidemic,       
    Pacifism,
    Terrorist,
    OpenArms,                 
    Xenophobia,               
    ExcessCharge,             
    PowerHungry,              
    NoiseComplaint,           
    WhiteNoiseToSleep,               
    Claustrophobia            
}


public class ElectionManager : MonoBehaviourSingleton<ElectionManager>
{
    public List<ElectionModifier> electionModifiers = new List<ElectionModifier>();
    public float goodwillModifier = 1.0f;

    private void Update()
    {
        CheckGoodwillConditions();
    }

    public void TriggerElection()
    {
        GameManager.Instance.currentElectionCycle += 1;

        // Determine the election modifiers
        ApplyElectionModifiers();

        // Generate the election message text
        string electionMessage = GenerateElectionMessage();

        // Show the election pop-up
        ElectionPopUp.Instance.ShowElectionPopUp(electionMessage);
    }

    private void ApplyElectionModifiers()
    {
        electionModifiers.Clear();

        ElectionModifier modifier1 = GetRandomModifier();
        ElectionModifier modifier2 = GetRandomModifier();

        electionModifiers.Add(modifier1);
        electionModifiers.Add(modifier2);

        //add more modifiers
        int additionalModifiersCount = Random.Range(0, 2);
        for (int i = 0; i < additionalModifiersCount; i++)
        {
            ElectionModifier additionalModifier = GetRandomModifier();
            electionModifiers.Add(additionalModifier);
        }
    }

    private ElectionModifier GetRandomModifier()
    {
        // Get a random election modifier
        ElectionModifier[] modifiers = (ElectionModifier[])System.Enum.GetValues(typeof(ElectionModifier));
        return modifiers[Random.Range(0, modifiers.Length)];
    }

    private string GenerateElectionMessage()
    {
        string message = "Election Results:\n";

        foreach (ElectionModifier modifier in electionModifiers)
        {
            message += $"{GetModifierDescription(modifier)}\n";
        }

        return message;
    }

    private string GetModifierDescription(ElectionModifier modifier)
    {
        switch (modifier)
        {
            case ElectionModifier.TooManyCooks:
                return "-Too Many Cooks > Dislikes crew quarters being close together";
            case ElectionModifier.LonelinessEpidemic:
                return "+Loneliness Epidemic > Likes crew quarters being far apart from each other";
            case ElectionModifier.Pacifism:
                return "+Pacifism > Dislikes when there are too many active turrets";
            case ElectionModifier.Terrorist:
                return "-Pacifism > Dislikes when there are too many active turrets";
            case ElectionModifier.OpenArms:
                return "+Open Arms > Likes having many refugees";
            case ElectionModifier.Xenophobia:
                return "-Xenophobia > Dislikes when there are too many refugees";
            case ElectionModifier.ExcessCharge:
                return "+Excess Charge > Likes or dislikes having many generators";
            case ElectionModifier.PowerHungry:
                return "+Wants more generators";
            case ElectionModifier.NoiseComplaint:
                return "-Noise Complaint > Dislikes when generators are too close to crew quarters";
            case ElectionModifier.WhiteNoiseToSleep:
                return "+I Need White Noise to Sleep > Likes generators close to crew quarters";
            case ElectionModifier.Claustrophobia:
                return "+Claustrophobia > Likes when there are more buffer rooms than non-buffer rooms";
            default:
                return "Unknown modifier";
        }
    }

    public void CheckGoodwillConditions()
    {
        goodwillModifier = 1;

        foreach (var modifier in electionModifiers)
        {
            switch (modifier)
            {
                case ElectionModifier.TooManyCooks:
                    if (CheckTooManyCooks())
                        ModifyGoodwill(-1);
                    break;
                case ElectionModifier.LonelinessEpidemic:
                    if (CheckLonelinessEpidemic())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.Pacifism:
                    if (CheckPacifism())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.Terrorist:
                    if (CheckTerrorist())
                        ModifyGoodwill(-1);
                    break;
                case ElectionModifier.OpenArms:
                    if (CheckOpenArms())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.Xenophobia:
                    if (CheckXenophobia())
                        ModifyGoodwill(-1);
                    break;
                case ElectionModifier.ExcessCharge:
                    if (CheckExcessCharge())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.PowerHungry:
                    if (CheckPowerHungry())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.NoiseComplaint:
                    if (CheckNoiseComplaint())
                        ModifyGoodwill(-1);
                    break;
                case ElectionModifier.WhiteNoiseToSleep:
                    if (CheckWhiteNoiseToSleep())
                        ModifyGoodwill(1);
                    break;
                case ElectionModifier.Claustrophobia:
                    if (CheckClaustrophobia())
                        ModifyGoodwill(1);
                    break;
            }
        }
    }

    // Methods to check specific conditions for each modifier
    private bool CheckTooManyCooks()
    {
        //check if crew quarters are too close together
        return GameManager.Instance.GetListOfRoomsTooCloseToRoomsOfSameType(RoomTypes.Crew, 1).Count > 0; 
    }

    private bool CheckLonelinessEpidemic()
    {
        // check if crew quarters are far apart
        Debug.Log(GameManager.Instance.GetListOfRoomsTooCloseToRoomsOfSameType(RoomTypes.Crew, 1).Count);
        return GameManager.Instance.GetListOfRoomsTooCloseToRoomsOfSameType(RoomTypes.Crew, 1).Count == 0;
    }

    private bool CheckPacifism()
    {
        // check if there are too many active turrets
        return GameManager.Instance.GetRoomCountOfRoomType(RoomTypes.Turret) > 6;
    }

    private bool CheckTerrorist()
    {
        // check if there are too less active turrets
        return GameManager.Instance.GetRoomCountOfRoomType(RoomTypes.Turret) < 6;
    }

    private bool CheckOpenArms()
    {
        // check if there are many refugees
        return ResourceManager.Instance.refugees <= 6.0f;
    }

    private bool CheckXenophobia()
    {
        //check if there are too many refugees
        return ResourceManager.Instance.refugees >= 6.0f;
    }

    private bool CheckExcessCharge()
    {
        //check if there are many generators
        return GameManager.Instance.GetRoomCountOfRoomType(RoomTypes.Generator) >= 8;
    }

    private bool CheckPowerHungry()
    {
        //check if there are not enough generators
        return GameManager.Instance.GetRoomCountOfRoomType(RoomTypes.Generator) < 8;
    }

    private bool CheckNoiseComplaint()
    {
        //check if generators are too close to crew quarters
        return GameManager.Instance.GetListOfRoomsOfTypeInRangeOfRoomsOfType(RoomTypes.Generator, RoomTypes.Crew, 1).Count > 0;
    }

    private bool CheckWhiteNoiseToSleep()
    {
        //check if generators are close to crew quarters
        return GameManager.Instance.GetListOfRoomsOfTypeInRangeOfRoomsOfType(RoomTypes.Crew, RoomTypes.Generator, 1).Count == GameManager.Instance.GetRoomCountOfRoomType(RoomTypes.Crew);
    }

    private bool CheckMeltingPot()
    {
        // Implement logic to check if storage rooms with refugees are close to crew quarters
        return false;
    }

    private bool CheckClaustrophobia()
    {
        // Implement logic to check if there are more buffer rooms than non-buffer rooms
        return GameManager.Instance.ShipHasMajorityBufferRooms();
    }

    // Method to modify goodwill
    private void ModifyGoodwill(int amount)
    {
        goodwillModifier += amount;
        Settings.Instance.GoodWillMultiplier = goodwillModifier;
        Debug.Log("Goodwill modified by: " + amount);
    }
}
