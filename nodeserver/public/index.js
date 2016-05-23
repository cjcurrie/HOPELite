var socket = io();

function FBSuccess(response)
{
  socket.emit('login', {'type':'facebook', 'fbUserID':response.userID, 'fbAccessToken':response.accessToken});
  //socket.emit('getContent', {'type':'morningQuestionnaire', 'auth':{'username':'cjcurrie'}});
}

function SendLogin()
{
  var params = {
    'type':'normal',
    'username':$('#username').val(),
    'password':$('#password').val()
  };

  socket.emit('login', params);
}

function ParseInput(input)
{
  $('#messages').append($('<li>').text(input));
  var first = input.split(':')[0];
  var second = input.split(':')[1];

  if (first && second)
    socket.emit(first, second);
  else if (first)
    socket.emit(first);
}

function ParseRemoteOption(option)
{
  switch (option['action'])
  {
    case 'login':
      var output = '<div class="login-form-container"><fieldset class="login-username"><input id="username" type="text" autocorrect="off" autocapitalize="none" autofocus="" required="" name="username" data-required="true" placeholder="Username or email (case sensitive)"></fieldset><fieldset class="login-password"><input id="password" type="password" name="password" data-required="true" placeholder="Password (case sensitive)"></fieldset><fieldset><button type="submit" title="Login" class="button-green submit-button" onclick="SendLogin()">Login</button></fieldset><fb:login-button scope="public_profile,email" onlogin="checkLoginState(FBSuccess);" /></div>';

      return output;
    break;

    case 'broadcast':
      return '<button type="button">'+option['text']+'</button>';
    break;

    default: return '';
  }
}


// === Server Events ===
socket.on('serverMsg', function(msg){
  $('#messages').append($('<li>').text(msg));
});

socket.on('login-response', function(msg){
  switch (msg)
  {
    case 'fbSuccess':
      $('status').val() = 'Logged in with Facebook account ' + msg['name'];
    break;
    case 'normalSuccess':
      $('status').val('Logged in as' + msg['name']);
      $('#messages').append($('<li>').text("Successfully logged in."));
    break;
    case 'normalBadUser':
      $('#messages').append($('<li>').text("Unknown username."));
    break;
    case 'normalBadPass':
      $('#messages').append($('<li>').text("Invalid password."));
    break;
  }
});

socket.on('optionList', function(options){
  var htmlString = 'Server offers the following actions:<div class="serverOption">';
  var count = 1;

  for (var opt in options)
  {
    //if (!p.hasOwnProperty(key)) continue;
    htmlString += '<h3>'+count.toString()+' <div style="display:inline-block;">'+ParseRemoteOption(options[opt])+'</div>';
  }
  htmlString += '</div>';
  $('#messages').append($('<li>').html(htmlString));
});