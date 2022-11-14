# ModernToDoList.Api
`To Do List ASP.NET Core Minimal Api`

Simple ToDoList C# project purposes were to practice with modern technologies, and to try completely new libraries for me.
I am trying my best in project architecture and clean code. 

> 🚧 Project is still in progress...

## 💻 Technologies used:
- Minimal Api with FastEndpoints
- Postgres Database in Docker
- Dapper (instead of heavy EF)
- FluentMigrator for migrations in database (only SQL Scripts)
- My Connection Pool implementation
- Swagger Doc
- Azure Storage Blob to persist photos/avatars
- BlurHash technology for blurred image preview before actual image
- Converting to WebP format with a little compression
- JWT Auth with modified ExpireAt
- My CRUD Repositories implementation
- BCrypt hashing passwords with SHA-256 with salt

## 📈 Plans for:
- Serilog logging + logging to AWS Cloud
- Redis for caching data/requests
- Desktop App on Electron/WPF with MVVM
- MediatR
- Polly for retries policy
- Figure out how auth in desktop apps works through browser
- RabbitMQ/socket.io for notifications or even live to do list editing idk
