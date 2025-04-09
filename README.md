## Project Goal

Exsta was created as both a learning vehicle and a showcase — a way to develop hands-on experience with the modern .NET and Azure ecosystem while building something meaningful and demonstrable.

The goal is twofold:

- **To demonstrate architectural and technical proficiency**  
  Exsta is designed from the ground up using real-world patterns like microservices, Domain-Driven Design, event-driven messaging, and secure API management. It showcases the ability to design, implement, and maintain a distributed system in a scalable and cloud-native way — covering topics like authentication, CI/CD, observability, and secure communication.

- **To create a professional-grade portfolio project**  
  Every decision — from tooling to code structure — is made with production-level quality in mind. The aim isn’t to build a toy app, but a full-featured system that could realistically support real-world business needs, while still being cost-conscious and maintainable in a solo-developer context.

Ultimately, Exsta is a proving ground: a place to explore best practices, document design decisions, and demonstrate fluency in delivering robust backend services and modern web applications — all within the Azure cloud.


## Tech Stack and Tooling

Exsta is built with a modern cloud-native stack, selected for reliability, scalability, and developer productivity. Below is an overview of the technologies and tools used to bring the system to life — including the reasoning behind key choices.

### Design & Philosophy

- **Domain Driven Design**  
  Exsta is structured around DDD principles. Each microservice aligns with a distinct domain and contains its own models, logic, and persistence. This helps maintain a clear separation of concerns, simplifies scaling and maintenance, and keeps the system adaptable as business needs evolve.

### Languages & Frameworks

- **C# / .NET 8**  
  All backend services are written in C# using the latest, generally available (at the time) .NET version. It's a fast, type-safe, and mature platform — ideal for building scalable APIs and background services.

- **Blazor (WebAssembly)**  
  The front-end communicates securely with the API layer. Brought to life by Blazor, making use of its components and responsiveness for an excellent user experiences. Picked because Blazor and .NET 8 provide a seamless full-stack C# development environment, minimizing context switching.

- **SQL (Azure SQL Database)**  
  Each microservice owns its own SQL database. This separation supports the microservices architecture and cleanly isolates data per domain.

### Cloud Platform

- **Microsoft Azure (PaaS-first)**  
  Exsta runs entirely in Azure, taking advantage of Platform-as-a-Service offerings:
  - **App Service** – for hosting public-facing APIs  
  - **Azure Functions** – for scheduled and event-driven background processing  
  - **Azure SQL** – scalable, managed database per service  
  - **Service Bus** & **Event Grid** – for robust messaging between services  
  - **Key Vault** – to securely manage secrets and connection strings  
  - **API Management (APIM)** – as the secured gateway to all APIs

### Authentication & Security

- **JWT (JSON Web Tokens)**  
  Secure and stateless user authentication and role-based authorization across the system.

- **API Management Gateway (APIM)**  
  Controls access using subscription keys and routes all external API calls. Internally, services communicate via secure messaging, not direct HTTP calls.

- **Local User Secrets & Azure Key Vault**  
  For development, secrets like connection strings are stored securely using [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets). In production and deployed enviroments, these are managed through Key Vault for secure secret management in production environments.

### Development Tools & Practices

- **Visual Studio / VS Code**  
  Day-to-day development happens in Visual Studio, with lightweight edits in VS Code as needed, such as this wiki page.

- **GitHub & GitHub Actions**  
  Full CI/CD setup with GitHub Actions: code is automatically built, tested, and deployed on push or pull requests.

- **Entity Framework Core**  
  Simplifies database access and schema migrations using a strongly typed, LINQ-based ORM.

- **Swagger / OpenAPI**  
  Automatically generated documentation makes testing and development easier for developers and testers.

- **Bruno & SSMS**  
  [Bruno](https://www.usebruno.com/) (a Postman-lookalike, but free) is used to manually test APIs, while SQL Server Management Studio helps inspect and troubleshoot data in real-time.

### Testing

- **xUnit & Moq**  
  Unit and integration tests ensure code quality. Dependencies are mocked with Moq, allowing for isolated and repeatable tests.

- **Automated Testing**  
  All tests are run automatically in CI on every pull request and commit to main to ensure changes don’t break functionality.

### Looking Ahead: Infrastructure & Observability

- **Infrastructure as Code (IaC)**  
  Plans are in place to integrate tools like **Bicep**, making deployments repeatable and maintainable, this is planned to take place after the first service is at its Minimum Viable Product stage, allowing it to act as a blueprint for future services.

- **Monitoring & Insights**  
  Currently using Azure Application Insights for logging and basic monitoring, with plans to expand into dashboards and alerts for deeper visibility once the project matures.

## Important considerations

Due to the nature of this project being one of a portfolio, low costs are an important aspect of the design choices. While Azure provides excellent options for low cost, to even free, usage of many of its largest offerings, some options are simply not available to Exsta. The following products were left off the design plans for such reasons:

- **Private endpoints and virtual networks**: While this is an excellent best practice for isolating a cluster of microservices to then make it accessible through an Application Gateway or APIM instance only, it is not available for use due to the cost required for usage.
