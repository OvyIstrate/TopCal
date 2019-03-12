# TopCal API

## Description

The Backend API used for meal registration web/mobile clients.

## Features
- Registers & manages users by roles (Admin, Manager, Regular)
- Stores "Meal" registrations


## Configuration

1. Add your MSSQL Server Connection string in the appSettings.json
```javascript
"ConnectionStrings": {
    "TopCalDb": "Data Source={{source}};Initial Catalog=TopCalDb;"
},
```
2. Add Application Settings Configuration used for generating JTW tokens and sending emails:
```javascript
"AppSettings": {
    "Token": {
      "Key": "{{token_key}}",
      "Issuer": "{{issuer}}",
      "Audience": "{{audience}}"
    },
    "EmailConfig": {
      "Host": "{{host}}",
      "Port": "{{port}}",
      "Username": "{{email-user}}",
      "Password": "{{email-secret}}" //this can be hashed or be a env variable name
    }
  },
```
