## API Specifications

* Output in Json or Xml format
* Output pagination and filtering
* ApiKey Authentication method for clients
* Swagger documentation

All the APIs output the following Json structures depending on whether the response is paged or not:

#### Response

```
{
    "success": true,
    "message": "",
    "errors": [],
    "data": {
        
    }
}
```

#### Paged Response

```
{
    "pageNumber": 1,
    "pageSize": 5,
    "recordsTotal": 0,
    "success": true,
    "message": "",
    "errors": [],
    "data": []
}
```

#### HTTP Status Codes

I always try to use the best HTTP Status Code for every type of response. In this particular project the following are the possible response code:

* OK 200
* Unauthorized 401
* Forbidden 403
* BadRequest 400
* NotFound 404
* InternalServerError 500
