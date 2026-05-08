# DarkAgeLegacy

## How to run

The game has two console projects:

- `DarkAgeLegacyServer` - server
- `DarkAgeLegacyClient` - client

Start the server first. Then start one or more clients.

## Run in Visual Studio

1. Open `DarkAgeLegacy.sln`.
2. Start `DarkAgeLegacyServer`.
3. Start `DarkAgeLegacyClient` in a second console window.
4. If you want more players, start `DarkAgeLegacyClient` again in another console window.

## Run from terminal

Open a terminal in the project folder and run the server:

```bash
dotnet run --project DarkAgeLegacy/DarkAgeLegacyServer.csproj
```

Open another terminal in the same project folder and run the client:

```bash
dotnet run --project DarkAgeLegacyClient/DarkAgeLegacyClient.csproj
```

For more players, open more terminals and run the client command again.

## Notes

- The server uses port `5000`.
- The client connects to `localhost:5000`.
- User accounts are saved locally by the server.
