# C# prac day task

## Create a .NET Web Api for managing a personal library.

_I would recommend that you use git on this repo, so that we can roll back to previous commits to test end points should there be any issues during testing._

The .NET Web Api should use user authentication to be able to manage personal libraries of multiple people on the same database.

## Task0:

Set up ssh for this repo on your system.
If you don't yet have an ssh key set up on your device you can follow these steps:

```bash
ssh-keygen -t rsa -b 2048 -C <Your computer name>
```

You can set up a password, and store in a custom directory but an empty password in the default directory is fine.
after this you can copy your key properties with one of these commands:
MacOS:

```
tr -d "\\n" < ~/.ssh/id_rsa.pub | pbcopy
```

Linux:

```
xclip -sel clip < ~/.ssh/id_rsa.pub
```

Git bash on windows:

```
cat ~/.ssh/id_rsa.pub | clip
```

and then set up your ssh key on your github profile

you can read up more on this at: https://docs.gitlab.com/ee/user/ssh.html

## Task1:

for db setup you can use docker:

```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssword1' -p 1433:1433 --name docker_msql -d mcr.microsoft.com/mssql/server:2019-latest
```

with the following connection string:

```
Server=127.0.0.1;Database=LibraryDb;MultipleActiveResultSets=true;User=sa;Password=P@ssword1;TrustServerCertificate=true
```
Create an AccountsController in this web api that can do the following:
Entity fields:

```
UserId: Guid
UserName: string
Password: string
```

- user registration
  example call: ` POST: https://localhost:5001/Accounts/register` with body:

```
{
    username: admin,
    password: 1q2w#E,
}
```

should return a 204 on success.

- user logon with JWT Bearer Authentication
  example call: ` POST: https://localhost:5001/Accounts/login` with body:

```
{
    username: admin,
    password: 1q2w#E,
}
```

should return a JWT token string upon success.

## Task2:

Create an Authors Controller with the basic CRUD (Create, Read, Update and Delete) actions on author entities.
Entity fields:

```
AuthorId: GUID
AuthorName: string
ActiveFrom: DateTime
ActiveTo: DateTime
CreatedBy: GUID(userId)
```

example api calls:
Create: ` POST: https://localhost:5001/Authors` with the body:

```
{
   authorName: "Tolkien",
    activeFrom: "1892-01-02",
    activeTo: "1973-09-03"
}
```

Read: ` GET: https://localhost:5001/Authors/{AuthorId}` & ` GET: https:localhost:5001/Authors` should return a specific instance and a full list of authors respectively
Update: ` PUT: https://localhost:5001/Authors/{AuthorId}` with the body:

```
{
    authorName: "J.R.R. Tolkien",
    activeFrom: "1892-01-02",
    activeTo: "1973-09-03"
}
```

Delete ` Delete: https://localhost:5001/Authors/{AuthorId}`

Do note, that any logged in user should be able to see all of the authors that have been stored, but you will only be able to update and delete an author if you have created the Author.

## Task3:

Create a Books Controller that can add books belonging to an author. (A book can only belong to one author)
Book Entity:

```
{
    BookId: Guid
    BookName: string,
    Publisher: string,
    DatePublished: DateTime,
    CopiesSold: int,
    Author: GUID(AuthorId)
    CreatedBy: GUID(UserId)
}
```

example API calls:
Create: ` POST: https://localhost:5001/Books/{AuthorId}` with the body:

```
{
    BookName: "Lord of the Rings",
    datePublished : "1970-01-01",
    publisher: "Harpercollins",
    copiesSold: 10000
}
```

Read: ` GET: https://localhost:5001/Books` should return :

```
{
    [
        bookName: string,
        bookAuthorName: string,
        ownsAuthor: bool
    ], ...
}
```

and ` GET: https://localhost:5001/Books/{AuthorId}` should return :

```
{
    [
        bookName: string,
        datePublished: DateTime
    ], ...
}
```

and lastly: ` GET: https://localhost:5001/Books/{AuthorId}/{BookId}` should return :

```
{
    bookName: string,
    datePublished: DateTime,
    publisher: string,
    copiesSold: int
    creatorName: string,
}
```

_You should be able to add books to an author regardless of whether you are the creator of an author._

## Problem solver question

Consider a wheel that has 26 sectors on them each corresponding to a letter of the alphabet.

- This wheel takes 5 seconds to rotate to an adjacent sector.
- This wheel can rotate in both directions
- The reader takes a double reading if no movement have been measured after 2.5 seconds.
- the wheel resets to the first sector after completing

Thus asking this wheel to write out a word would take a certain amount of time, which is a function of the characters in the word it needs to write out.

example:

```
The time it would take to write out: ABC would be 2.5 + 5 + 5 = 12.5 seconds.
The time it would take to write out CBA would be 10 + 5 + 5 = 20 seconds.
The time it would take to write out the word DEEZ would be 47.5 seconds:
15 (move from A to D) +
5 (move from D to E) +
2.5 (stay at E to double register) +
25 (Move backwards from E to Z) seconds
```

write out a method in C# that can calculate the time it would take to write out any string comprising of the characters [A..Z].
_you can bind this to an endpoint on your library project for quick testing if you like._
