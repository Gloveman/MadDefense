const fs=require('fs');
var wsServer=require("ws").Server
var wss=new wsServer({port:5001},()=>{console.log("listening 5001....")})

let map={
    data:[
        {
            points:[-5,0],
            tile:1
        },
        {
            points:[-4,0],
            tile:1
        },
        {
            points:[-3,0],
            tile:1
        },
        {
            points:[-2,0],
            tile:1
        },
        {
            points:[-1,0],
            tile:1
        },
        {
            points:[0,0],
            tile:1
        },
        {
            points:[5,-0],
            tile:1
        },
        {
            points:[4,0],
            tile:1
        },
        {
            points:[3,0],
            tile:1
        },
        {
            points:[2,0],
            tile:1
        },
        {
            points:[1,0],
            tile:1
        }
    ]
}

wss.on('connection',(client)=>{

    // client.on("message",(data)=>{
    //     if(data=="목록")

    // });
    client.send(JSON.stringify(map));
});