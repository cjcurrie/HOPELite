// Imports
var http = require('http').Server(app);
var app = require('express')();
var io = require('socket.io');

// Definitions
const PORT=2358; 

app.listen(PORT, function(){
  console.log('listening on *:'+PORT);
});

var ios = io.listen(app);

// Request/Response handling
ios.sockets.on('connection', function(socket){
  console.log('a user connected');

  // Register disconnect callback
  socket.on('disconnect', function(){
    console.log('user disconnected');
  });

  socket.on('message', function(msg){
    console.log('message: ' + msg);
  });
});