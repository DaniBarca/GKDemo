using System.Threading.Tasks;
using Apple.Core.Runtime;
using Apple.GameKit;
using UnityEngine;

public class GameKitManager : MonoBehaviour
{
    public static GameKitManager Instance { get; private set; }
    
    public GKLocalPlayer LocalPlayer { get; private set; } = null;
    public NSArray<GKPlayer> Friends { get; private set; }

    public bool IsAuthenticated => LocalPlayer?.IsAuthenticated ?? false;
    public bool IsUnderage => LocalPlayer?.IsUnderage ?? false;
    public bool IsMultiplayerGamingRestricted => LocalPlayer?.IsMultiplayerGamingRestricted ?? false;
    public bool IsPersonalizedCommunicationRestricted => LocalPlayer?.IsPersonalizedCommunicationRestricted ?? false;
    
    private void Awake()
    {
        Instance ??= this;
    }
    
    private async Task Start()
    {
        try
        {
            LocalPlayer = await GKLocalPlayer.Authenticate();
            
            Debug.Log(@$"[GameKitManager] LocalPlayer: {LocalPlayer} 
    -- teamPlayerId: {LocalPlayer.TeamPlayerId}
    -- gamePlayerId: {LocalPlayer.GamePlayerId}
    -- isUnderage: {LocalPlayer.IsUnderage}
    -- isMultiplayerGamingRestricted: {LocalPlayer.IsMultiplayerGamingRestricted}
    -- isPersonalizedCommunicationRestricted: {LocalPlayer.IsPersonalizedCommunicationRestricted}");

            Friends = await LocalPlayer.LoadFriends();
            var friendsMessage = "[GameKitManager] Friends: ";
            foreach (var friend in Friends)
            {
                friendsMessage += $"{friend.Alias} -- ({friend.TeamPlayerId}) -- ({friend.GamePlayerId})";
            }
            
            Debug.Log(friendsMessage);
        }
        catch
        {
            Debug.LogError($"[GameKitManager] Failed to authenticate local player");
        }
    }
}