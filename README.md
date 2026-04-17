# 🍸 Cocktail Collator

## 📖 Description
Cocktail Collator is a web application designed to manage, organise, and create cocktail recipes (or recipes of any kind). 

It makes finding the specific recipe that you are looking for much easier, by allowing you to search through your recipes by filters that make sense to _you_.
It allows streamlining of the process to make a recipe, by allowing you to create and edit on the spot, using predefined settings to your liking, which in turn makes it easier for you to find them later. 
You have the ability to maintain specific settings like the names of measurements, ingredient categories, and types of recipes.

<br>

The application is built with a focus on maintainability, scalability, and clean architecture principles, making it easy to extend and evolve over time.

## 🚀 Installation

Cocktail Collator is distributed as a Docker container. 
As long as you have a working Docker setup, you shouldn't need any dependencies.

### 📦 Images

- Image: `ghcr.io/volublerobin7/cocktail-collator`
- Tags:
  - `latest` – Latest stable release  
  - `dev` – Latest development build (There is no need to use this unless you are wanting to contribute to the project. Any help is greatly appreciated!)

### 🎯 Variables

Mounts:
- `/var/opt/mssql` - This is where the database is stored, to make sure that you keep your data even after the container closes, this is needed.

Ports:
- 7012 - This is the port that the webpage is exposed on.

Environment Variables:
- TODO

### 📸 Example

A typical compose file could look something like this:
```yaml
services:
    cocktailcollator:
        container_name: cocktailcollator
        image: ghcr.io/volublerobin7/cocktailcollator:latest
        restart: unless-stopped
        volumes:
          - ./cocktailcollator:/var/opt/mssql
        ports:
          - "7012:7012"
```

## 🏗️ Project Structure & Architecture

### 🧱 Architecture Overview

- Architecture Style: Clean Architecture and MVVM
- Key Principles:
  - Separation of concerns
  - Dependency inversion
  - Testability and modularity
  - Scalability 

#### 📦 Application Layers TODO

- Presentation Layer
  -  Built with Blazor Server
  -  Handles UI rendering and user interaction
- Application Layer
  - Contains core use cases and business logic
  - Coordinates between domain and infrastructure
- Domain Layer
  - Core entities and business rules
  - Independent of external frameworks
- Infrastructure Layer
  - Data access (Entity Framework Core)
  - External integrations and persistence

#### 🧩 Design Patterns TODO

- Dependency Injection
  - Used throughout to decouple components
- Repository Pattern
  - Abstracts data access logic
- Observer Pattern
  - Used in UI state handling (e.g. collection change events)

#### 🗄️ Data Access

ORM: Entity Framework Core <br>
Supports: MS-SQL Server

#### ⚙️ Key Technologies

- ASP .NET (Blazor Server)
- Entity Framework Core
- AutoMapper
- Docker