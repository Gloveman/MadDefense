
var wsServer=require("ws").Server
var wss=new wsServer({port:5001},()=>{console.log("listening 5001....")})

let map={
    data:[
        {
            points:[-5,-3],
            tile:1
        },
        {
            points:[-4,-3],
            tile:1
        },
        {
            points:[-3,-3],
            tile:1
        },
        {
            points:[-2,-3],
            tile:1
        },
        {
            points:[-1,-3],
            tile:1
        },
        {
            points:[0,-3],
            tile:1
        },
        {
            points:[5,-3],
            tile:1
        },
        {
            points:[4,-3],
            tile:1
        },
        {
            points:[3,-3],
            tile:1
        },
        {
            points:[2,-3],
            tile:1
        },
        {
            points:[1,-3],
            tile:1
        }
    ]
}

wss.on('connection',(client)=>{

    client.send(JSON.stringify(map));
});