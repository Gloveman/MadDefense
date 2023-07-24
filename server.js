const fs=require('fs');
var wsServer=require("ws").Server
var wss=new wsServer({port:5001},()=>{console.log("listening 5001....")})

// let map={
//     data:[
//         {
//             points:[-5,0],
//             tile:1
//         },
//         {
//             points:[-4,0],
//             tile:1
//         },
//         {
//             points:[-3,0],
//             tile:1
//         },
//         {
//             points:[-2,0],
//             tile:1
//         },
//         {
//             points:[-1,0],
//             tile:1
//         },
//         {
//             points:[0,0],
//             tile:1
//         },
//         {
//             points:[5,-0],
//             tile:1
//         },
//         {
//             points:[4,0],
//             tile:1
//         },
//         {
//             points:[3,0],
//             tile:1
//         },
//         {
//             points:[2,0],
//             tile:1
//         },
//         {
//             points:[1,0],
//             tile:1
//         }
//     ]
// }


wss.on('connection',(client)=>{

     client.on("message",(data)=>{
        var parsed=JSON.parse(data);
        if(parsed.ContentType=="savemap"){
            
            fs.writeFileSync("./Maps/"+parsed.name,data);
        }
        else if(parsed.ContentType=="selectmap")
        {
            var names=[]
            var times=[]
            var levels=[]
            let res={}
            fs.readdir("./Maps",(err,files)=>{
                files.forEach(
                    (file)=>{
                        let data=fs.readFileSync("./Maps/"+file);
                        var parsed=JSON.parse(data);
                        names.push(parsed.name);
                        times.push(parsed.time);
                        levels.push(parsed.level);
                    }
                );
                res.names=names;
                res.times=times;
                res.levels=levels;
                client.send(JSON.stringify(res));
            });
        }
        else {
            let map=fs.readFileSync("./Maps/"+parsed.name);
            client.send(map.toString());
        }

    });
    //send map

});