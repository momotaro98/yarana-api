# Yarana API

Yarana API is a api for yarana client like [yarana-bot](https://github.com/momotaro98/yarana-bot).

This api runs on Azure Functions and connects Cosmos DB.

## API Schemes

```
GET
/kotos?userId=0123456789abcdefghijklmnopqrstuvwxyz
POST
/koto
GET
/activities?kotoId=0123456789abcdefghijklmnopqrstuvwxyz
POST
/activity
```

## Koto model

```
{
  "id": "string",
  "userId": "string",
  "title": "string"
}
```

## Activity model

```
{
  "id": "string",
  "kotoId": "string",
  "timestamp": "string"
}
```
