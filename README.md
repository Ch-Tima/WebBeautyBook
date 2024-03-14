# BeautyBook

[BeautyBook](https://appwebbeautybook.azurewebsites.net/) is a web service designed for beauty salons, providing a convenient tool for online appointment booking for clients and an effective means for promoting their services.

## Main Idea

BeautyBook provides clients with instant access to thousands of services that meet their needs, along with a convenient online registration process for the selected service. It's a modern solution that combines the best aspects of the beauty industry and online technologies.

## Technologies Used
<div align="center">  
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/csharp.svg" width="55" height="55">
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/angularjs.svg" width="55" height="55">
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/bootstrap.svg" width="55" height="55">
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/azure.svg" width="55" height="55">
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/sendgrid.svg" width="55" height="55">
  <img src="https://raw.githubusercontent.com/Ch-Tima/Ch-Tima/fc68b93af9b33d6c183ea885663c82cc04d3fe26/images/jwt.svg" width="55" height="55">
</div>

- **C#**: Primary development language for the API.
- **MSSQL**: Database.
- **ASP.NET API Core**: Technology used to build the API.
- **HTML/CSS**: Frontend development.
- **Bootstrap**: Framework for rapid website styling.
- **Angular**: Framework for building the frontend.
- **Microsoft Azure**: Cloud platform.
- **SendGrid**: Service for quick message sending.

## Project Architecture

The project is built on a multi-layered architecture:

- **[Domain](https://github.com/Ch-Tima/WebBeautyBook/tree/master/Domain)**: Contains business entities and data models.
- **[DAL](https://github.com/Ch-Tima/WebBeautyBook/tree/master/DAL)**: Responsible for interacting with the database.
- **[BLL](https://github.com/Ch-Tima/WebBeautyBook/tree/master/BLL)**: Business logic layer, intermediate between presentation and data access. It [configures](https://github.com/Ch-Tima/WebBeautyBook/blob/master/BLL/Infrastructure/ConfigrationBLL.cs#L21) DB, Identity, JWT, and token provider.
- **[WebAPI](https://github.com/Ch-Tima/WebBeautyBook/tree/master/WebBeautyBook)**: Provides access to the application's functionality via HTTP requests and responses.

## Security and Authentication

To ensure security, I use JWT and ASP.NET Core Identity.

## Integration with SendGrid and Azure

I have successfully integrated the SendGrid service for quick message sending. I also utilize the Microsoft Azure cloud platform for deploying and scaling the application.

## How to run a project:
  - [Visual Studio](https://github.com/Ch-Tima/WebBeautyBook/wiki/How-to-run-a-project#visual-studio)


## Activity
![Alt](https://repobeats.axiom.co/api/embed/33c7eba0395b029383be1af49fcbcd15242a6f53.svg "Repobeats analytics image")

## Developer

- **Name**: Tymofii
- **GitHub**: [Ch-Tima](https://github.com/Ch-Tima/)
