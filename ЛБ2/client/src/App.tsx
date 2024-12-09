/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import {
  Message,
  MyError,
  WebsocketData,
  WebsocketDataType,
} from "./typedefs/websocketData";
import { Button, Input, Typography } from "@material-tailwind/react";

const socket = new WebSocket("ws://localhost:8080");

function App() {
  const [username, setUsername] = useState("");
  const [usernameInput, setUserNameInput] = useState("");
  const [messageInput, setMessageInput] = useState("");
  const [error, setError] = useState("");
  const [messages, setMessages] = useState<Message[]>([]);

  useEffect(() => {
    socket.onopen = function () {
      console.log("Connection is up");
    };

    socket.onmessage = (response: MessageEvent<any>) => {
      let parsedData: WebsocketData;
      console.log(response);

      try {
        parsedData = JSON.parse(response.data);
      } catch (error) {
        console.log("Couldn't parse data", error);
        return;
      }

      switch (parsedData.type) {
        case WebsocketDataType.Message: {
          const result = parsedData as Message;
          setMessages((prev) => [...prev, result]);
          return;
        }
        case WebsocketDataType.Error: {
          const result = parsedData as MyError;
          setError("Error: " + result.payload.message);
          return;
        }
        default:
          return;
      }
    };
  }, []);

  const sendMessage = () => {
    if (!messageInput) return;

    const message: Message = {
      type: WebsocketDataType.Message,
      payload: {
        nickname: username,
        message: messageInput,
        timestamp: JSON.stringify(Date.now()),
      },
    };
    console.log("Message sent");

    socket.send(JSON.stringify(message));
    setMessageInput("");
    setError("");
  };

  return (
    <>
      {username ? (
        <div className="relative flex flex-col h-screen bg-gray-900 text-gray-200">
          {/* Sidebar for username display */}
          <div className="absolute top-0 left-0 w-48 h-full bg-gray-800 p-5 flex flex-col items-center border-r border-gray-700">
            <Typography className="text-xl font-bold mb-4 text-neon-green">
              {username}
            </Typography>
            <Button
              className="mt-auto bg-red-500 hover:bg-red-600 text-white"
              onClick={() => setUsername("")}
            >
              Logout
            </Button>
          </div>

          {/* Chat Header */}
          <div className="w-full flex justify-center bg-gray-700 py-4">
            <Typography className="text-2xl font-bold text-neon-blue">
              Chat Room
            </Typography>
          </div>

          {/* Chat messages */}
          <div className="flex flex-col flex-1 overflow-y-auto p-5 ml-48 mt-4 gap-4">
            {!messages.length && (
              <div className="flex justify-center items-center h-full text-gray-500 text-xl">
                No messages yet...
              </div>
            )}

            {error && (
              <div className="text-red-400 text-right pr-4">{error}</div>
            )}

            {!!messages.length && (
              <div className="flex flex-col gap-3">
                {messages.map((message) => {
                  const { nickname, message: messageText, timestamp } =
                    message.payload;
                  return (
                    <div
                      key={timestamp}
                      className="relative flex flex-col gap-1 p-4 border border-gray-700 rounded bg-gray-800 shadow-md"
                    >
                      <div className="text-neon-green font-semibold">
                        {nickname}
                      </div>
                      <div className="text-gray-200">{messageText}</div>
                      <div className="absolute bottom-2 right-2 text-xs text-gray-500 font-semibold">
                        {timestamp}
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>

          {/* Floating message input */}
          <div className="fixed bottom-5 left-1/2 transform -translate-x-1/2 w-3/4 flex">
            <input
              className="flex-1 p-3 text-gray-900 outline-none border border-gray-700 rounded-l bg-gray-100"
              placeholder="Type a message..."
              value={messageInput}
              onChange={(e) => setMessageInput(e.target.value)}
            />
            <Button
              className="rounded-r bg-neon-blue hover:bg-blue-600"
              onClick={() => sendMessage()}
            >
              Send
            </Button>
          </div>
        </div>
      ) : (
        <div className="flex h-screen items-center justify-center bg-gray-900 text-gray-200">
          <div className="text-center p-8 bg-gray-800 rounded shadow-lg border border-gray-700">
            <h1 className="text-xl font-bold mb-4 text-neon-green">
              Enter Your Username
            </h1>
            <div className="flex gap-2">
              <Input
                placeholder="John Doe"
                className="bg-gray-700 text-white border border-gray-500"
                onChange={(e) => setUserNameInput(e.target.value)}
              />
              <Button
                className="bg-neon-blue hover:bg-blue-600"
                onClick={() => setUsername(usernameInput)}
              >
                Join
              </Button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}

export default App;
