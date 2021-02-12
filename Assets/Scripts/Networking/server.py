import socket, random
from _thread import *

class Game:
    def __init__(self, code, maxPlayers, host):
        self.code = code
        self.maxPlayers = maxPlayers
        self.host = host    # id of host of game

        self.playerIDs = [] # list of player ids in game

        self.active = False
        self.full = False
        self.turn = 0
        self.counter = 0

    def AddPlayer(self, id):
        self.playerIDs.append(id)
        if len(self.playerIDs) >= self.maxPlayers:
            self.full = True
            print("Game full")
        print(f"Added player: {id}, now player IDs: {self.playerIDs}")
        return id

    def Disconnect(self, id):
        self.playerIDs.remove(id)
        self.full = False
        if len(self.playerIDs) > 0 and id == self.host:
            self.host = self.playerIDs[0]
        print(f"Removed player: {id}, now player IDs: {self.playerIDs}")

    def Encode(self):
        return str(self.turn) + "~" + str(self.counter) + "~" + str(self.active) + "~" + str(self.maxPlayers)

    def Decode(self, msg):
        msg = msg.split("~")
        self.turn = msg[0]
        self.counter = msg[1]

VERSION = "0.0"

server = "192.168.1.104"
port = 5555

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

try:
    s.bind(('',port))
except socket.error as e:
    print(str(e))

s.listen()
print("Waiting for a connection...")

connected = set()
games = {}
players = -1

codeCharacterValues = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'

def threaded_client(conn, playerID):
    gameCode = ""
    while True:
        try:
            msg = conn.recv(1024).decode().split("::")
            response = "null"

            if msg[0] == "version_check":
                if msg[1] == VERSION:
                    response = "true"
                else:
                    if float(VERSION) > float(msg[1]):
                        response = "false_c"
                    else:
                        response = "false_s"

            elif msg[0] == "create_game":
                while True:
                    code = random.choice(codeCharacterValues) + random.choice(codeCharacterValues) + random.choice(codeCharacterValues) + random.choice(codeCharacterValues)
                    if code not in games:
                        games[code] = Game(code, int(msg[1]), playerID)
                        break
                print(f"new game created with code {code}")
                response = code

            elif msg[0] == "get_game":
                if msg[1] in games:
                    response = games[msg[1]].Encode()

            elif msg[0] == "get_game_info":
                if msg[1] in games:
                    response = games[msg[1]].code + " " + str(len(games[msg[1]].playerIDs)) + " " + str(games[msg[1]].maxPlayers)

            elif msg[0] == "get_game_code":
                if msg[1] in games:
                    response = msg[1]
                    if games[msg[1]].full:

                        response = "full"
            elif msg[0] == "join_game":
                if not games[msg[1]].full and not games[msg[1]].active:
                    response = str(games[msg[1]].AddPlayer(playerID)) 
                    gameCode = games[msg[1]].code
                    print(f"{playerID} {addr} joined game {gameCode}")
                elif games[msg[1]].full:
                    response = "full"

            elif msg[0] == "send_game":
                games[msg[1]].Decode(msg[2])

            elif msg[0] == "start_game":
                if not games[msg[1]].active and games[msg[1]].full:
                    games[msg[1]].active = True
                    response = "true"
                    print(f"started game {msg[1]}")
            
            elif msg[0] == "leave_game":
                games[msg[1]].Disconnect(playerID)
                if len(games[gameCode].playerIDs) <= 0:
                    games.pop(gameCode)
                    response = "true"
                    print(f"Game {gameCode} deleted.")

            elif msg[0] == "get_host":
                if msg[1] in games:
                    response = str(games[msg[1]].host == playerID)

            conn.send(response.encode())
        except:
            print(f"{playerID} {addr} disconnected forcefully.")
            # print(e)
            break
    if gameCode in games:
        games[gameCode].Disconnect(playerID)
        if len(games[gameCode].playerIDs) <= 0:
            games.pop(gameCode)
            print(f"Game {gameCode} deleted.")
    conn.close()

while True:
    conn, addr = s.accept()
    if players < 999999:
        players += 1
    else:
        players = 0
    print("Connected to: ", addr)
    start_new_thread(threaded_client, (conn, players))