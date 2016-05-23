var AWS = require("aws-sdk");

AWS.config.update({
    region: "us-west-2",
    endpoint: "dynamodb.us-west-2.amazonaws.com"
  });

const TABLENAME = 'HOPElite', USERINDEX='username-index', FBINDEX = 'fbID-index';
var dynamodb = new AWS.DynamoDB();


// === Log In/New User/Delete User ===
exports.TryLogin = function(username, password, callback)
{
  // First query to see if item is in the index
  var params = {
    TableName: TABLENAME,
    IndexName: USERINDEX,
    KeyConditionExpression: "username = :user",
    ExpressionAttributeValues: {
        ":user": {S: username}
    },
    ProjectionExpression: "userid"
  };

  dynamodb.query(params, function(err, data){
    if (err || !data.Items[0]){
      callback("normalBadUser");
    }
    else if (data)
    {
      // Then getItem to retrieve user data for the given username
      params = {
        TableName: TABLENAME,
        AttributesToGet: [
          "password", "userid"
        ],
        Key : { 
          "userid" : {
            "N" : data.Items[0].userid.N
          }
        }
      };

      dynamodb.getItem(params, function(err, resp){
        if (err){console.log("database error getting user data: "+err);}
        else if (resp)
        {
          if (resp.Item.password.S != password){callback("normalBadPass");}
          else
          {
            callback("normalSuccess");
          }
        }
      });
    }
  });
}

// == Query Table ===

exports.Scan = function(callback)
{
  var params = {
    TableName: TABLENAME
  };
  dynamodb.scan(params, function(err,data){
    if (err){console.log(err, err.stack);  callback(err);}
    else
    {
      callback("count: "+data.Count+ " – "+JSON.stringify(data.Items, null, 2));
    }
  });
}

exports.CreateMasterUser = function(callback){
  console.log("creating master user");
  // @TODO: add conditional to check if item already exists
  var params = {
    Item: {
      'userid': {N: '0'},
      'username': {S: 'master'},
      'password': {S: 'health'},

      'dob': {S: '5/17/2016'}
    },
    TableName: TABLENAME,
    ReturnConsumedCapacity: 'TOTAL',
    ReturnItemCollectionMetrics: 'SIZE',
    ReturnValues: 'ALL_OLD'
  };
  dynamodb.putItem(params, function(err, data) {
    if (err){console.log(err, err.stack);callback(err);}
    else
    {
      callback("Data that was replaced: "+JSON.stringify(data, null, 2));
    }
  });
}

exports.GetItem = function(user, callback){
  var params = {
    Key: {
      'username': {S: user}
    },
    TableName: TABLENAME,
    ConsistentRead: false,  // For strongly consistent reads set true
    //ProjectionExpression: 'STRING_VALUE',   // Only set this to get a specific set of attributes. Otherwise, all are returned
    ReturnConsumedCapacity: 'TOTAL'
  };
  dynamodb.getItem(params, function(err, data) {
    if (err){console.log(err, err.stack);callback(err);}
    else
    {
      callback(JSON.stringify(data, null, 2));
    }
  });
}

exports.DBQuery = function(callback){
  var docClient = new AWS.DynamoDB.DocumentClient();

  console.log("Querying for movies from 1992 - titles A-L, with genres and lead actor");

  var params = {
      TableName : "HOPELite",
      ProjectionExpression:"#yr, title, info.genres, info.actors[0]",
      KeyConditionExpression: "#yr = :yyyy and title between :letter1 and :letter2",
      ExpressionAttributeNames:{
          "#yr": "year"
      },
      ExpressionAttributeValues: {
          ":yyyy":1992,
          ":letter1": "A",
          ":letter2": "L"
      }
  };

  docClient.query(params, function(err, data) {
      if (err) {console.log("Unable to query. Error:", JSON.stringify(err, null, 2));callback(JSON.stringify(err, null, 2));}
      else {
          // data.Items.forEach(function(item) {
          //     console.log(" -", item.year + ": " + item.title
          //     + " ... " + item.info.genres
          //     + " ... " + item.info.actors[0]);
          // });
          callback(JSON.stringify(data, null, 2));
      }
  });
}

exports.ListTables = function(callback)
{
  var params;
  dynamodb.listTables(params, function(err, data) {
    if (err){console.log(err, err.stack); callback(err);} 
    else
    {
      callback(JSON.stringify(data, null, 2));
    }
  });
}

exports.CreateTable = function(callback){
  var params = {
    AttributeDefinitions: [
      {
        AttributeName: 'username',
        AttributeType: 'S'
      },
      {
        AttributeName: 'userid',
        AttributeType: 'N'
      },
      {
        AttributeName: 'dob',
        AttributeType: 'S'
      }
    ],
    KeySchema: [
      {
        AttributeName: 'username',
        KeyType: 'HASH'
      }
    ],
    ProvisionedThroughput: {
      ReadCapacityUnits: 10,
      WriteCapacityUnits: 10
    },
    TableName: TABLENAME,
    StreamSpecification: {
      StreamEnabled: true,
      StreamViewType: 'KEYS_ONLY'
    }
  };
  dynamodb.createTable(params, function(err, data) {
    if (err){console.log(err, err.stack); callback(err);} 
    else
    {
      callback(JSON.stringify(data, null, 2));
    }
  });
}