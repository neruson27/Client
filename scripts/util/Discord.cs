using DiscordRPC;

public class Discord
{
    private static string appId = "1231699688340590722";
    public static DiscordRpcClient Client { get; private set; }

    static Discord()
    {
        // No Discord client to talk to on mobile, Client stays null
        if (Godot.OS.HasFeature("mobile"))
        {
            return;
        }

        // TODO: Add logging here
        Client = new DiscordRpcClient(appId) { };

        Client.Initialize();

        Client.SetPresence(new RichPresence() { Assets = new Assets() { LargeImageKey = "short" } });
    }
}
