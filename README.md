# ICT2112 P1 Module 1 Ordering Processing System Merged Web Application (Team 4 and Team 5) 


## 📌 Breakdown of Features  

Team 4 is responsible for development and implementation of staff-side functionalities of this system. The primary focus is on streamlining internal operations, ensuring that CBC staff can efficiently manage orders, process refunds, handle customer support inquiries, and facilitate communication with external shipping agents. The expected outcomes include improving order tracking accuracy, optimising refund processing, enhancing delivery process visibility, and providing a structured support ticketing system.

Team 5 is responsible for developing an E-commerce System with a focus on delivering core functionalities that enhance the user experience and streamline the management of Customer interactions. Our portion of the project includes essential features that will allow users to interact with products, manage their accounts, and handle orders effectively.


## 🚀 How to Run the Project  

Running this project will require Visual Studio 2022 Community Edition or Visual Studio Studio Code with the C# Extension. The project runs on **.NET 9.0 SDK**.    
&nbsp;

### 1️⃣**📥 Download the Project**
**Option 1: Manual Download (ZIP)**  
1. Click the green **"Code"** button.
2. Select **"Download ZIP"**.
3. Extract the ZIP file to your preferred location.

**OR**

**Option 2: Using Git CLI**

Make sure you have **Git** installed.  
If not, you can download it from [git-scm.com](https://git-scm.com/).

```
git clone https://github.com/ICT2112SoftwareDesign/ICT2112-Software-Design-ClassP1/tree/module1-merge
```

## 2️⃣**📦 Required NuGet Packages**

This project requires the following NuGet packages:

| Package | Description |
| :--- | :--- |
| **Google.Cloud.Dialogflow.V2** | Access Dialogflow V2 API from .NET applications. |
| **Microsoft.AspNetCore.Identity** | ASP.NET Core Identity framework for authentication and authorization. |
| **Microsoft.Data.SqlClient** | SQL Server data provider for .NET Core and .NET Framework. |
| **System.Data.SqlClient** | Legacy SQL Server data provider for .NET Framework applications. |

**Screenshot of NuGet Packages Solution GUI:**

![Screenshot 2025-04-05 105030](https://github.com/user-attachments/assets/a9df4bc1-f41d-4623-9779-78953df0fa7b)

You can also install the required packages via CLI by running the following commands in your project directory:

```
dotnet add package Google.Cloud.Dialogflow.V2
dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.Data.SqlClient
dotnet add package System.Data.SqlClient
```
&nbsp;
## 3️⃣**📂 Add additional fies to the project**
Add the following file into to the project repository:
```
teak-clone-454005-d5-5fa367197d61.json
```

Additionally, **replace** the 'appsettings.json' file and **add** in the 'teak-clone-454005-d5-5fa367197d61.json' file, provided in Team 5's .zip` folder. Specific instructions and screenshots can be found in Team 5's D2 Report. As this GitHub repository is public, the database connection strings and API keys are excluded from this branch.
&nbsp;

## 4️⃣**⚙️ Run the project**


**Running with Visual Studio 2022 Community Editio**n

1. Open the `.sln` (Solution) file in Visual Studio 2022.
2. Make sure the correct Startup Project is selected (right-click the project > **Set as Startup Project**).
3. Press **F5** (or click **https** button).
 

**OR**

**Running with Visual Studio Code**

1. Open the project folder (where the `.csproj` file is located) in **VS Code**.
2. Open a new terminal inside VS Code.
3. Run the application:
```
dotnet run
```
## 📢 Contact, Contributions and Acknowledgements
If there are issues with running the branch, please reach out to **ICT2112 P1 Team 4 and 5**. We would like to thank Prof. Serena and Prof. Francis for their feedback and guidance for this module!
