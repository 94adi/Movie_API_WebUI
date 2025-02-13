# MovieApp (API & WebUI) (Under construction)

## Description

This project is an IMDb clone that allows users to browse movies, read detailed descriptions, rate and review movies, view other user reviews, and watch trailers. It was built with scalability in mind, where the backend (API) and frontend (WebUI) are separate instances, using modern web development strategies and enabling independent scaling to handle increased traffic. The architecture ensures flexibility and performance, making it suitable for future growth.


## Table of Contents
- [Business/Practical Aspect](#businesspractical-aspect)
- [Technical Overview](#technical-overview)
- [Architecture Overview](#architecture-overview)
- [Features](#features)
- [Technologies](#technologies)

## Business/Practical Aspect

The primary motivation behind this project was to leverage my web development experience and expand upon it by constructing a RESTful API with JWT authentication from the ground up. A key goal was to have a WebUI communicate with this API via HTTP calls for seamless integration. Hosting the entire application in Azure was also a significant objective, which involved using Azure Key Vault for secure secret and configuration storage, and Azure File Share for poster file support. Deploying separate App Services for the API and UI provided valuable insights into migrating a modern application to the cloud.
  
## Technical Overview  

The system follows a **scalable, decoupled architecture**, ensuring maintainability and performance. The backend and frontend communicate via **RESTful APIs**, while Azure services provide **secure storage and hosting**.  

- **Backend:** A `.NET-based RESTful API` handles business logic, authentication (`JWT, ASP.NET Identity`), and data persistence using `Entity Framework Core`. `MediatR` is integrated to decouple controllers from business services, following the CQRS pattern where applicable. API documentation is available via `Swagger (OpenAPI)`.  
- **Frontend:** A `server-side rendered WebUI` built with `ASP.NET Core MVC`, `JavaScript`, and `Bootstrap`. The UI interacts with the API via `HTTPS`, and `MediatR` is used to manage interactions and improve modularity.  
- **Database:** A `SQL` database stores movie details, user reviews, and ratings.  
- **Cloud Services:** The entire system is **hosted in Azure**, leveraging **App Services** for API & UI deployments.  
- **Security:** Authentication is implemented using **JWT** and `HMACSHA256`. Sensitive secrets are managed via **Azure Key Vault**, and all communication is secured with **HTTPS**.  
- **Storage:** Movie posters are stored and retrieved via **Azure File Share**, ensuring scalability and availability.  


## Architecture Overview

![MovieApp drawio3](https://github.com/user-attachments/assets/25f983c7-ff4a-4c6d-902a-07dd04d706b1)

### **Components:**
- **Web UI:** Communicates with the API via HTTPS calls.
- **RESTful API:** Handles authentication, data retrieval from other Azure services, and business logic.
- **Azure Key Vault:** Manages secrets and configuration securely.
- **Azure Storage:** Stores poster images and other media.
- **Azure SQL Database:** Stores data for: movies, users and more.
- **App Services:** Separate instances for API and UI, allowing independent scaling.

## Features

- **Browse Movies:** View a list of movies and search for specific titles.
- **Movie Details:** Read detailed descriptions, genres, and release dates.
- **Rate & Review:** Users can rate and review movies.
- **User Reviews:** View reviews from other users.
- **Watch Trailers:** Embedded trailers for a better experience.

## Technologies  

The project utilizes a **modern tech stack** to ensure scalability, security, and maintainability:  

### **Backend**  
- **.NET Core** – RESTful API development  
- **Entity Framework Core** – ORM for database interactions  
- **ASP.NET Identity** – User authentication & authorization  
- **MediatR** – Implements CQRS for better separation of concerns  
- **Swagger (OpenAPI)** – API documentation  
- **JWT (JSON Web Tokens)** – Secure token-based authentication  

### **Frontend**  
- **ASP.NET Core MVC** – Server-side rendered UI  
- **JavaScript** – Dynamic client-side interactions  
- **Bootstrap** – Responsive design & styling  
- **MediatR** – Improves modularity by decoupling UI components from direct service calls  

### **Cloud & Hosting**  
- **Azure App Services** – Hosting for the API and Web UI  
- **Azure Key Vault** – Secure storage for secrets & configurations  
- **Azure File Share** – Storage for movie posters
- **Azure SQL Database** – Relational database for storing movies, users, and reviews      

### **Security**  
- **HMACSHA256** – Secure token signing  
- **HTTPS** – Encrypted communication  

### **Other Tools & Services**  
- **Docker** – Containerized development
- **Postman** – API testing & debugging  
