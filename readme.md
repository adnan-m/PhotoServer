
# Photo Service

## Task

Create a Web API that when called
1) Calls, combines and returns the result of:
- http://jsonplaceholder.typicode.com/photos
- http://jsonplaceholder.typicode.com/albums
2) Allows an integrator to filter on the user id - so just returns the albums and photos relevant to a single user.

## Server

Start RunPathPhotoServer via IISExpress

There are two different end points

### Returns combine Photo and Alubm data

- [https://localhost:44366/api/PhotoAlbum](https://localhost:44366/api/PhotoAlbum) 

### Returns combine Photo and Alubm data by User

- [https://localhost:44366/api/PhotoAlbum/user/1](https://localhost:44366/api/PhotoAlbum/user/1) 

### Return Data Format:

**PhotoAlbum:**
```
{
	album: Album,
	photos: Photo[]
}
```

**Album:**
```
{
	id: int,
	userId: int,
	title: string
}
```

**Photo:**
```
{
	id: int,
	albumId: int,
	title: string,
	url: Uri,
	thumbnail: Uri
}
```

