package main

import (
	"log"
	"net/http"

	"encoding/json"
	"fmt"
	"github.com/gorilla/websocket"
)

type Message struct {
	Type string
	Data MessageData
}

type MessageData struct {
	Player string
	X, Y   float64
}

var upgrader = websocket.Upgrader{}

func socketHandler(w http.ResponseWriter, r *http.Request) {
	log.Print("got request")
	setResponseHeaders(w.Header(), r.Header.Get("Origin"))

	c, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Print("upgrade:", err)
		return
	}
	defer c.Close()
	consumeMessages(c)
}

func setResponseHeaders(header http.Header, origin string) {
	header.Set("Access-Control-Allow-Origin", origin)
	header.Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
	header.Set("Access-Control-Allow-Credentials", "true")
	header.Set("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, User-Agent, Connection")
	header.Set("Content-Type", "application/json")
}

func consumeMessages(c *websocket.Conn) {
	for {
		msg, err := receiveMessage(c)
		if err != nil {
			log.Println("receive:", err)
			break
		}

		fmt.Printf("%+v\n", msg)
	}
}

func receiveMessage(c *websocket.Conn) (Message, error) {
	_, message, err := c.ReadMessage()
	if err != nil {
		return Message{}, err
	}

	var msg Message
	err = json.Unmarshal(message, &msg)
	if err != nil {
		return Message{}, err
	}

	return msg, nil
}

func main() {
	upgrader.CheckOrigin = func(r *http.Request) bool {
		return true
	}
	log.SetFlags(0)
	http.HandleFunc("/", socketHandler)
	log.Fatal(http.ListenAndServe(":8080", nil))
}
