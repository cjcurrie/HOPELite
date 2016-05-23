var express = require('express');
var app = express();
app.use('/', express.static(__dirname + '/public'));
var http = require('http').Server(app);
var io = require('socket.io')(http);
var AWS = require('aws-sdk');

// --- Local scripts
var db = require('./scripts/DBHelper.js');

// === Config ===
const PORT = 8080;

app.get('/', function(req, res){
  res.sendFile('index.html');
});


// === Server startup ===
http.listen(PORT, function(){
  console.log('listening on *:8080');
});


// === Sockets ===
io.on('connection', function(socket){
  console.log('a user connected');

  // Connection sequence
  socket.emit('serverMsg', 'Welcome to HOPELite!');
  
  socket.emit('optionList', {
    opt1:{text:'Log in', action:'login'},
    opt2:{text:'Say Hello to everyone', action:'broadcast'}
  });
  
  // === Register Callbacks ===
  socket.on('disconnect', function(){
    console.log('user disconnected');
  });
  socket.on('getContent', function(req){
    switch(req['type'])
    {
      case 'morningQuestionnaire':
        console.log("fetching morning content for "+req['auth'].username);
        
      break;
      case 'eveningQuestionnaire':

      break;
    }
  });

  socket.on('login', function(req)
  {
    switch (req['type'])
    {
      case 'normal':
        db.TryLogin(req['username'], req['password'], function(status){
          console.log(status);
          socket.emit('login-response', status);
        });
      break;
      case 'facebook':
      break;
      default:
        console.log("error: bad login request");
    }

    // @TODO: extend short-term fb user access token into long-term?
  });


  // Testing commands
  socket.Respond = function(msg){
    socket.emit('serverMsg', msg);
  }
  socket.on('scan', function(){
    db.Scan(socket.Respond);
  });
  socket.on('get', function(){
    db.GetItem('cjcurrie', socket.Respond);
  });
  socket.on('put', function(){
    db.PutItem(socket.Respond);
  });
  socket.on('createMaster', function(){
    db.CreateMasterUser(socket.Respond);
  });
  socket.on('list', function(){
    db.ListTables(socket.Respond);
  });
  socket.on('create table', function(){
    db.CreateTable(socket.Respond);
  });
});


// === Other server functions ===
process.on('uncaughtException', function (err) {
    console.log(err);
}); 

var gracefulShutdown = function() {
  console.log("\nReceived kill signal, shutting down gracefully.");
  http.close(function() {
    console.log("Closed out remaining connections.");
    process.exit()
  });
  
   //if after shutdown fails
   setTimeout(function() {
       console.error("Could not close connections in time, forcefully shutting down");
       process.exit()
  }, 1*1000);
}


// ============== Shell support ==================
// Command typed into shell 
process.on ('SIGTERM', gracefulShutdown);
// ctrl-c 
process.on ('SIGINT', gracefulShutdown);   