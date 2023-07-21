var wsServer=require("ws").Server
var wss=new wsServer({port:5001},()=>{console.log("listening 5001....")})


wss.on('connection',(client)=>{
    client.on('message',(message)=>{
        console.log("client: %s",message)
    });
    client.send("From node.js");
});