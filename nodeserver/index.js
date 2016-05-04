var express = require('express');
var app = express();
app.use('/', express.static(__dirname + '/public'));
var http = require('http').Server(app);
var io = require('socket.io')(http);

app.get('/', function(req, res){
  res.sendFile('index.html');
});

io.on('connection', function(socket){
  console.log('a user connected');

  // Connection sequence
  socket.emit('serverMsg', 'Welcome to HOPELite.');
  
  socket.emit('optionList', {
    opt1:{text:'Log in', action:'login'},
    opt2:{text:'Say Hello to everyone', action:'broadcast'}
  });
  
  // === Register Callbacks ===
  socket.on('disconnect', function(){
    console.log('user disconnected');
  });
  socket.on('clientMsg', function(msg){
    ParseMsg(socket, msg);
  });
});

function ParseMsg(socket, msg)
{
  console.log(msg);
}

http.listen(8080, function(){
  console.log('listening on *:8080');
});