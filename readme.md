An Asp.Net CORE Web API application with JWT Authentication. 

## To Run the Application

* Install dotnet core
* Then run the following commands in terminal
* `dotnet ef database update`
* `dotnet run`

## Available endpoints

```
GET - AuthRequired - api/users
GET - AuthRequired - api/users/{id}
```
* Include the authentication token in the `Authorization` header in the format `Bearer {token}`


```
POST - auth/login

// post body
{
	"username": "name",
	"password": "pass"
}
```


```
POST - auth/register

// post body
{
    "username": "name",
    "firstName": "optional_",
    "lastName": "optional_",
    "email": "optional_",
    "password": "pass"
}

```
