using Mono.Data.Sqlite;

public static class AppData
{
    public static int activePlayerID = -1; // -1 is equivilant to null;
    public static string activeGunName = "Pistol";
    public static string dbName = "game.db";

    public static void DBConnect(out SqliteConnection connection, out SqliteCommand cmd)
    {
        connection = new SqliteConnection("URI=file:" + dbName);
        connection.Open();
        cmd = connection.CreateCommand();
    }
    public static void DBClose(ref SqliteConnection connection, ref SqliteCommand cmd)
    {
        cmd.Dispose();
        connection.Close();
        connection.Dispose();
    }
}
